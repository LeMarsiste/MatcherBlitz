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
    private float levelTimeLimit = 600;
    private float currentTime = 0;


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
    public void SetGameTheme(int index)
    {
        Sprite BackgroundSprite = DataCenter.Instance.ThemePresets[index].BackgroundSprite;
        UIController.Setbackground(BackgroundSprite);
    }
    #endregion

    #region Private

    private void EndTheGame()
    {

    }
    #endregion
    #endregion
}
