using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    #region Members
    #region Public
    [Header("Hierarchy Objects")]
    public TextMeshProUGUI LevelText;

    #endregion
    #region Private

    #endregion
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Level_Index"))
            PlayerPrefs.SetInt("Level_Index", 1);
        int currentLevel = PlayerPrefs.GetInt("Level_Index");
        LevelText.text = "Level " + currentLevel;
    }
    #endregion




    public void StartLevel()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("CLICKED");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
