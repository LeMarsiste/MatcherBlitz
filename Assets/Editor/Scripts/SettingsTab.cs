
using UnityEditor;
using UnityEngine;


/// <summary>
/// The "Game settings" tab in the editor.
/// </summary>
public class SettingsTab : EditorTab
{

    private Vector2 scrollPos;

    private int newLevel;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="editor">The parent editor.</param>
    public SettingsTab(MatcherBlitzEditor editor) : base(editor)
    {
        newLevel = PlayerPrefs.GetInt("Level_Index");
    }

    /// <summary>
    /// Called when this tab is drawn.
    /// </summary>
    public override void Draw()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 100;

        GUILayout.Space(15);

        DrawPreferencesTab();

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Draws the preferences tab.
    /// </summary>
    private void DrawPreferencesTab()
    {
        GUILayout.Space(15);

        DrawPreferencesSettings();

    }

    /// <summary>
    /// Draws the preferences settings.
    /// </summary>
    private void DrawPreferencesSettings()
    {
        EditorGUILayout.LabelField("Level", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Level", "The current level number."),
            GUILayout.Width(50));
        newLevel =
            EditorGUILayout.IntField(newLevel, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Set progress", GUILayout.Width(120), GUILayout.Height(30)))
        {
            PlayerPrefs.SetInt("Level_Index", newLevel);
        }

        GUILayout.Space(15);

        EditorGUILayout.LabelField("PlayerPrefs", EditorStyles.boldLabel);
        if (GUILayout.Button("Delete PlayerPrefs", GUILayout.Width(120), GUILayout.Height(30)))
        {
            PlayerPrefs.DeleteAll();
        }

        GUILayout.Space(15);

        EditorGUILayout.LabelField("EditorPrefs", EditorStyles.boldLabel);
        if (GUILayout.Button("Delete EditorPrefs", GUILayout.Width(120), GUILayout.Height(30)))
        {
            EditorPrefs.DeleteAll();
        }
    }

}

