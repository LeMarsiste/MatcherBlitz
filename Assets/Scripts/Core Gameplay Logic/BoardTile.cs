using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public int index;
    public int PairedIndex;
    public Sprite NormalSprite;
    public Sprite ClickedSprite;
    SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        renderer.sprite = ClickedSprite;
        UIController.Instance.OnTileClicked?.Invoke(index);
    }

    public void ResetTile()
    {
        renderer.sprite = NormalSprite; 
    }

}
