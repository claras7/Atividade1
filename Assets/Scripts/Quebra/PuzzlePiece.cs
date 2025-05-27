using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    public Image image;
    public int correctIndex;
    public int currentIndex;

    public void OnClick()
    {
        PuzzleGameManager.Instance.OnPieceClicked(this);
    }

    public void Setup(Sprite puzzleSprite, int i)
    {
        image.sprite = puzzleSprite;   
        correctIndex = i;              
        currentIndex = i;  
    }
}
