using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemePreset", menuName = "ScriptableObjects/ThemePreset", order = 1)]
public class ThemeScriptableObject : ScriptableObject
{
    public LevelTheme Theme;
    public Sprite CardbackSprite;
    public Sprite BackgroundSprite;
}
