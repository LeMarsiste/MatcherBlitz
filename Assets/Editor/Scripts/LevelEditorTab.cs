
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Text.RegularExpressions;
using log4net.Core;
using NUnit.Framework.Internal;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

/// <summary>
/// The "Level editor" tab in the editor.
/// </summary>
public class LevelEditorTab : EditorTab
{
    private int prevWidth = -1;
    private int prevHeight = -1;

    private TileType currentTileType;

    private enum BrushMode
    {
        Tile,
        Row,
        Column,
        Fill
    }

    private BrushMode currentBrushMode = BrushMode.Tile;
    private readonly Dictionary<string, Texture> tileTextures = new Dictionary<string, Texture>();
    private LevelData currentLevel;

    private Vector2 scrollPos;
    private string currentLevelPath;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="editor">The parent editor.</param>
    public LevelEditorTab(MatcherBlitzEditor editor) : base(editor)
    {
        var editorImagesPath = new DirectoryInfo(Application.dataPath + "/Editor/Resources");
        var fileInfo = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly);
        foreach (var file in fileInfo)
        {
            var filename = Path.GetFileNameWithoutExtension(file.Name);
            tileTextures[filename] = Resources.Load(filename) as Texture;
        }
    }

    /// <summary>
    /// Called when this tab is drawn.
    /// </summary>
    public override void Draw()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 90;

        GUILayout.Space(15);

        DrawMenu();

        if (currentLevel != null)
        {
            var level = currentLevel;
            prevWidth = level.Width;

            GUILayout.Space(15);

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);

            GUILayout.BeginVertical();
            DrawGeneralSettings();
            GUILayout.EndVertical();

            GUILayout.Space(50);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Space(15);

            DrawLevelEditor();
        }

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Draws the menu.
    /// </summary>
    private void DrawMenu()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("New", GUILayout.Width(100), GUILayout.Height(50)))
        {
            currentLevel = new LevelData();
            //InitializeNewLevel();
        }

        if (GUILayout.Button("Open", GUILayout.Width(100), GUILayout.Height(50)))
        {
            var path = EditorUtility.OpenFilePanel("Open level", Application.dataPath + "/Resources/Levels",
                "json");
            if (!string.IsNullOrEmpty(path))
            {
                currentLevel = LoadJsonFile<LevelData>(path);

                currentLevelPath = path;
            }

        }

        if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(50)))
        {
            SaveLevel(Application.dataPath + "/Resources");
        }

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the general settings.
    /// </summary>
    private void DrawGeneralSettings()
    {
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        EditorGUILayout.HelpBox(
            "The general settings of this level.",
            MessageType.Info);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Level number", "The number of this level."),
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentLevel.ID = EditorGUILayout.IntField(currentLevel.ID, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Time", "The maximum number of seconds of this level."),
            GUILayout.Width(EditorGUIUtility.labelWidth));

        currentLevel.TimeLimit = EditorGUILayout.IntField(currentLevel.TimeLimit, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    /// <summary>
    /// Draws the level editor.
    /// </summary>
    private void DrawLevelEditor()
    {
        EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        EditorGUILayout.HelpBox(
            "The layout settings of this level.",
            MessageType.Info);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Width", "The width of this level."),
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentLevel.Width = EditorGUILayout.IntField(currentLevel.Width, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        prevHeight = currentLevel.Height;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Height", "The height of this level."),
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentLevel.Height = EditorGUILayout.IntField(currentLevel.Height, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Tile", "The current type of the tiles."),
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentTileType = (TileType)EditorGUILayout.EnumPopup(currentTileType, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Brush mode", "The current brush mode."),
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentBrushMode = (BrushMode)EditorGUILayout.EnumPopup(currentBrushMode, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (prevWidth != currentLevel.Width || prevHeight != currentLevel.Height)
        {

            currentLevel.Tiles = new List<LevelTile>(currentLevel.Width * currentLevel.Height);
            for (var i = 0; i < currentLevel.Width; i++)
            {
                for (var j = 0; j < currentLevel.Height; j++)
                {
                    currentLevel.Tiles.Add(new LevelTile() { Type = TileType.Normal });
                }
            }
        }
        GUILayout.Space(10);

        for (var i = 0; i < currentLevel.Height; i++)
        {
            GUILayout.BeginHorizontal();
            for (var j = 0; j < currentLevel.Width; j++)
            {
                var tileIndex = (currentLevel.Width * i) + j;
                CreateMainGridButton(tileIndex);
            }
            GUILayout.EndHorizontal();
        }
    }
    /// <summary>
    /// Creates a new tile button.
    /// </summary>
    /// <param name="tileIndex">The tile index.</param>
    private void CreateMainGridButton(int tileIndex)
    {
        var tileTypeName = string.Empty;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = MakeTexture(Color.gray);
        buttonStyle.active.background = MakeTexture(Color.white);
        if (currentLevel.Tiles[tileIndex] != null)
        {
            var blockTile = currentLevel.Tiles[tileIndex];
            tileTypeName = blockTile.Type.ToString();
        }

        if (tileTextures.ContainsKey(tileTypeName))
        {
            if (GUILayout.Button(tileTextures[tileTypeName], buttonStyle, GUILayout.Width(60), GUILayout.Height(60)))
            {
                DrawMainGridTile(tileIndex);
            }
        }
        else
        {
            if (GUILayout.Button("", buttonStyle, GUILayout.Width(60), GUILayout.Height(60)))
            {
                DrawMainGridTile(tileIndex);
            }
        }
    }

    /// <summary>
    /// Draws the tile at the specified index.
    /// </summary>
    /// <param name="tileIndex">The tile index.</param>
    private void DrawMainGridTile(int tileIndex)
    {
        var x = tileIndex % currentLevel.Width;
        var y = tileIndex / currentLevel.Width;

        switch (currentBrushMode)
        {
            case BrushMode.Tile:
                currentLevel.Tiles[tileIndex] = new LevelTile { Type = currentTileType };
                break;

            case BrushMode.Row:
                for (var i = 0; i < currentLevel.Width; i++)
                {
                    var idx = i + (y * currentLevel.Width);
                    currentLevel.Tiles[idx] = new LevelTile { Type = currentTileType };
                }
                break;

            case BrushMode.Column:
                for (var j = 0; j < currentLevel.Height; j++)
                {
                    var idx = x + (j * currentLevel.Width);
                    currentLevel.Tiles[idx] = new LevelTile { Type = currentTileType };
                }
                break;

            case BrushMode.Fill:
                for (var j = 0; j < currentLevel.Height; j++)
                {
                    for (var i = 0; i < currentLevel.Width; i++)
                    {
                        var idx = i + (j * currentLevel.Width);
                        currentLevel.Tiles[idx] = new LevelTile { Type = currentTileType };
                    }
                }
                break;
        }

    }


    /// <summary>
    /// Saves the current level to the specified path.
    /// </summary>
    /// <param name="path">The path to which to save the current level.</param>
    public void SaveLevel(string path)
    {
#if UNITY_EDITOR
        SaveJsonFile(path + "/Levels/" + currentLevel.ID + ".json", currentLevel);
        AssetDatabase.Refresh();
#endif
    }
    private Texture2D MakeTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}

