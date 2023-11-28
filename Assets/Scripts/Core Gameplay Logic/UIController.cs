using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    #region Members
    #region Public
    [Header("Hierarchy Objects")]
    public GameManager GameManager;
    [Space(10)]
    public Image BackgroundImage;
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
    }
    #endregion

    #region Methods
    #region Public
    public void Setbackground(Sprite backgroundSprite) => BackgroundImage.sprite = backgroundSprite;
    #endregion

    #region Private

    #endregion
    #endregion
}
