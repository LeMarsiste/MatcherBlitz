using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DataCenter : MonoBehaviour
{
    #region Members
    #region Public
    public LevelData CurrentLevelData;
    #endregion


    #region Private


    #endregion
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        int currentLevel = PlayerPrefs.GetInt("Level_Index");
        fsSerializer fsSerializer = new fsSerializer();
        CurrentLevelData = FileUtils.LoadJsonFile<LevelData>(fsSerializer,
                "Levels/" + currentLevel);

    }
    #endregion

    #region Methods
    #region Public


    #endregion

    #region Private

    #endregion
    #endregion
}
