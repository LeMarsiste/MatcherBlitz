using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData 
{
    public int ID;

    public int Height;
    public int Width;
    public int TimeLimit;
    public LevelTheme LevelTheme;

    public List<LevelTile> Tiles = new List<LevelTile>();
}
