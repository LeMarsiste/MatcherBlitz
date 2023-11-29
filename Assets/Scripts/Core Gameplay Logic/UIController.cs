using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    #region Members
    #region Public
    [Header("Hierarchy Objects")]
    public GameManager GameManager;
    [Space(10)]
    public Image BackgroundImage;

    [HideInInspector] public UnityEvent<int> OnTileClicked;
    #endregion


    #region Private


    #endregion
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        OnTileClicked.AddListener(onTileClicked);
    }
    #endregion

    #region Methods
    #region Public
    public void Setbackground(Sprite backgroundSprite) => BackgroundImage.sprite = backgroundSprite;

    public void ResetTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
            tile.GetComponent<BoardTile>().ResetTile();
    }
    public void DeleteTiles(List<GameObject> tiles)
    {
        foreach (GameObject obj in tiles)
            Destroy(obj);
    }
    #endregion

    #region Private
    private void onTileClicked(int index)
    {
        GameManager.TileClicked(index);
    }
    #endregion
    #endregion
}
