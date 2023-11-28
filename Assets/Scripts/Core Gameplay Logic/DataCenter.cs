using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class DataCenter : MonoBehaviour
{
    public static DataCenter Instance { get; private set; }

    #region Members
    #region Public
    [Header("Hierarchy Objects")]
    public GameManager GameManager;
    [Space(10)]
    public GameObject TilePools;
    public ObjectPool DefaultTilePool;
    public Transform BoardCenter;


    [Header("Presets")]
    public List<ThemeScriptableObject> ThemePresets;

    [HideInInspector] public LevelData CurrentLevelData;
    [HideInInspector] public readonly List<Vector3> tilePositions = new List<Vector3>();
    #endregion
    #region Private
    private List<GameObject> Tiles = new List<GameObject>();

    private Dictionary<LevelTheme, int> themeIndices = new Dictionary<LevelTheme, int>();
    private float tileW;
    private float tileH;

    #endregion
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        if (!PlayerPrefs.HasKey("Level_Index"))
            PlayerPrefs.SetInt("Level_Index", 1);
        int currentLevel = PlayerPrefs.GetInt("Level_Index");
        fsSerializer fsSerializer = new fsSerializer();
        CurrentLevelData = FileUtils.LoadJsonFile<LevelData>(fsSerializer,
                "Levels/" + currentLevel);
        

        for (int i = 0; i < ThemePresets.Count; i++)
        {
            int index = i;
            themeIndices[ThemePresets[i].Theme] = index;
        }

        RestartLevel();
    }
    #endregion

    #region Methods
    #region Public
    public void RestartLevel()
    {
        Tiles = new List<GameObject>(CurrentLevelData.Width * CurrentLevelData.Height);

        foreach (ObjectPool pool in TilePools.GetComponentsInChildren<ObjectPool>())
        {
            pool.Reset();
        }

        GameManager.SetGameTheme(themeIndices[CurrentLevelData.LevelTheme]);

        tilePositions.Clear();

        const float horizontalSpacing = 0.2f;
        const float verticalSpacing = 0.2f;

        List<GameObject> blockerTiles = new List<GameObject>();

        for (int j = 0; j < CurrentLevelData.Height; j++)
        {
            for (int i = 0; i < CurrentLevelData.Width; i++)
            {
                LevelTile CurrentLevelDataTile = CurrentLevelData.Tiles[i + (j * CurrentLevelData.Width)];
                GameObject tile = CreateTileFromLevel(CurrentLevelDataTile, CurrentLevelData.LevelTheme, i, j);
                if (tile != null)
                {
                    tile.name = "Tile i:" + i.ToString() + " j:" + j;


                    var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                    tileW = 1.3f;
                    tileH = 1.3f;
                    tile.transform.position =
                        new Vector2(i * (tileW + horizontalSpacing), -j * (tileH + verticalSpacing));
                }

                Tiles.Add(tile);
            }
        }
        var totalWidth = (CurrentLevelData.Width - 1) * (tileW + horizontalSpacing);
        var totalHeight = (CurrentLevelData.Height - 1) * (tileH + verticalSpacing);

        for (var j = 0; j < CurrentLevelData.Height; j++)
        {
            for (var i = 0; i < CurrentLevelData.Width; i++)
            {
                var tilePos = new Vector2(i * (tileW + horizontalSpacing), -j * (tileH + verticalSpacing));
                var newPos = tilePos;
                newPos.x -= totalWidth / 2;
                newPos.y += totalHeight / 2;
                newPos.y += BoardCenter.position.y;
                var tile = Tiles[i + (j * CurrentLevelData.Width)];
                if (tile != null)
                {
                    tile.transform.position = newPos;
                }
                tilePositions.Add(newPos);
                var CurrentLevelDataTile = CurrentLevelData.Tiles[i + (j * CurrentLevelData.Width)];

            }
        }
        float zoomLevel = 1.25f;

        var maxWidthHeight = Mathf.Max(totalWidth, totalHeight);

        var cameraMultiplier = (maxWidthHeight * zoomLevel) > 12 ? (maxWidthHeight * zoomLevel) : 12; // (totalWidth * zoomLevel) but as if the width is at least 9 blocks
        Camera.main.orthographicSize = cameraMultiplier * (Screen.height / (float)Screen.width) * 0.5f;

    }


    #endregion

    #region Private
    private GameObject CreateTileFromLevel(LevelTile levelTile, LevelTheme theme, int x, int y)
    {
        if (levelTile is LevelTile) //We can add other tiles in the future!
        {
            LevelTile leveltile = levelTile;
            switch (leveltile.Type)
            {
                case TileType.Golden:
                case TileType.Explosive:
                case TileType.Normal:
                    var tile = DefaultTilePool.GetObject();
                    tile.GetComponent<BoardTile>().x = x;
                    tile.GetComponent<BoardTile>().y = y;
                    tile.GetComponent<SpriteRenderer>().sprite = ThemePresets[themeIndices[theme]].CardbackSprite;
                    return tile;

                case TileType.Hole:
                    return null;
            }
        }

        return null;
    }
    #endregion
    #endregion
}
