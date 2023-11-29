using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    #region Members
    #region Public
    [Header("Hierarchy Objects")]
    public UIController UIController;
    public DataCenter DataCenter;
    [Space(10)]
    public ObjectPool tilePool;

    #endregion


    #region Private
    private List<Vector2> Pairs = new List<Vector2>();

    private float levelTimeLimit = 600;
    private float currentTime = 0;

    private int currentClickedTile = -1;
    private Queue<int> clickedTilesQueue = new Queue<int>();

    #endregion
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
    }
    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > levelTimeLimit)
            EndTheGame();
    }

    #endregion

    #region Methods
    #region Public
    public void RandomizeTheCards(List<GameObject> tiles)
    {
        List<int> availableIndices = new List<int>(),
            availableSpriteIndices = new List<int>();
        for (int i = 0; i < tiles.Count; i++)
            if (tiles[i] != null)
                availableIndices.Add(i);
        for (int i = 0; i < DataCenter.MaxSpriteIndex; i++)
            availableSpriteIndices.Add(i);

        bool isOddLevel = availableIndices.Count % 2 == 1;
        if (isOddLevel)
        {
            int index = Random.Range(0, availableIndices.Count);
            availableIndices.RemoveAt(index);
        }

        while (availableIndices.Count > 0)
        {
            int pair1Index = Random.Range(0, availableIndices.Count);
            GameObject pair1Object = tiles[availableIndices[pair1Index]];
            availableIndices.RemoveAt(pair1Index);

            int pair2Index = Random.Range(0, availableIndices.Count);
            GameObject pair2Object = tiles[availableIndices[pair2Index]];
            availableIndices.RemoveAt(pair2Index);

            int spriteIndex = Random.Range(0, availableSpriteIndices.Count);
            Sprite sprite = DataCenter.GetTileImage(spriteIndex);
            availableSpriteIndices.RemoveAt(spriteIndex);

            if(availableSpriteIndices.Count == 0) //For now ... have to find a better way to handle large maps, currently the max map size is 50 active tiles
                for (int i = 0; i < DataCenter.MaxSpriteIndex; i++)
                    availableSpriteIndices.Add(i);

            PairTiles(pair1Object, pair2Object,sprite);
        }

    }
    public void SetGameTheme(int index)
    {
        Sprite BackgroundSprite = DataCenter.ThemePresets[index].BackgroundSprite;
        UIController.Setbackground(BackgroundSprite);
    }
    public void TileClicked(int tileIndex)
    {
        clickedTilesQueue.Enqueue(tileIndex);
        if (clickedTilesQueue.Count == 1) 
            StartCoroutine(tileClicked());
    }
    IEnumerator tileClicked()
    {
        yield return new WaitForSeconds(1f);
        int tileIndex = clickedTilesQueue.Dequeue();
        if (currentClickedTile == -1)
        {
            currentClickedTile = tileIndex;
        }
        else
        {
            List<GameObject> tiles = new List<GameObject>();
            tiles.Add(DataCenter.GetTile(tileIndex));
            tiles.Add(DataCenter.GetTile(currentClickedTile));
            if (Pairs.Contains(new Vector2(currentClickedTile, tileIndex)) ||
                Pairs.Contains(new Vector2(tileIndex, currentClickedTile)))
                UIController.DeleteTiles(tiles);
            else
                UIController.ResetTiles(tiles);
            currentClickedTile = -1;
        }
        if (clickedTilesQueue.Count != 0)
            StartCoroutine(tileClicked());
    }
    #endregion

    #region Private

    private void EndTheGame()
    {

    }
    private void PairTiles(GameObject pair1, GameObject pair2,Sprite clickedSprite)
    {
        BoardTile pair1Tile = pair1.GetComponent<BoardTile>(),
            pair2Tile = pair2.GetComponent<BoardTile>();
        pair1Tile.ClickedSprite = clickedSprite;
        pair2Tile.ClickedSprite = clickedSprite;
        pair1Tile.PairedIndex = pair2Tile.index;
        pair2Tile.PairedIndex = pair1Tile.index;
        Pairs.Add(new Vector2(pair1Tile.index, pair2Tile.index));

    }
    
    #endregion
    #endregion
}
