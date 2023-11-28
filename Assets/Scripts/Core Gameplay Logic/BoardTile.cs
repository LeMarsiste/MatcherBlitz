using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int index;
    public int PairedIndex;

    

    private void OnMouseDown()
    {
        UIController.Instance.OnTileClicked?.Invoke(index);
    }

}
