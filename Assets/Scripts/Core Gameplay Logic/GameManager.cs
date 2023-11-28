using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Members
    #region Public
    public int LevelTimeLimit = 600;

    #endregion


    #region Private
    private float currentTime = 0;


    #endregion
    #endregion

    #region Unity Callbacks
    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > LevelTimeLimit)
            EndTheGame();
    }

    #endregion

    #region Methods
    #region Public


    #endregion

    #region Private
    private void EndTheGame()
    {

    }
    #endregion
    #endregion
}
