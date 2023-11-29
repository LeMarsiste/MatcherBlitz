using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Slider HealthSlider;
    public TextMeshProUGUI ScoreText;
    public Canvas Canvas;

    [Header("Prefabs")]
    public GameObject LosePrefab;
    public GameObject WinPrefab;

    [HideInInspector] public UnityEvent<int> OnTileClicked;
    [HideInInspector] public UnityEvent<float> OnHealthChanged;
    [HideInInspector] public UnityEvent<int> OnScoreChanged;
    #endregion


    #region Private
    private int currentScore = 0;

    #endregion
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        OnTileClicked.AddListener(onTileClicked);
        OnHealthChanged.AddListener(onHealthChanged);
        OnScoreChanged.AddListener(onScoreChanged);
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
    public void ShowWinScreen()
    {
        GameObject winScreen = Instantiate(WinPrefab, Canvas.transform);
    }
    public void ShowLoseScreen()
    {
        GameObject loseScreen = Instantiate(LosePrefab, Canvas.transform);
    }
    #endregion

    #region Private
    private void onTileClicked(int index)
    {
        GameManager.TileClicked(index);
    }
    private void onHealthChanged(float value)
    {
        HealthSlider.value = value;
    }
    private void onScoreChanged(int value)
    {
        currentScore += value;
        ScoreText.text = currentScore.ToString();
    }
    #endregion
    #endregion
}
