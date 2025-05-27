using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager Instance;

    public GameObject piecePrefab;
    public Transform boardParent;
    public Sprite[] puzzleSprites;
    public GameObject victoryPanel; // Painel de vitória na UI

    public List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    private PuzzlePiece selectedPiece = null;
    private Stack<ICommand> commandStack = new Stack<ICommand>();
    private List<ICommand> replayList = new List<ICommand>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateBoard();
        ShuffleBoard();
        victoryPanel.SetActive(false);
    }

    void CreateBoard()
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }
        pieces.Clear();

        for (int i = 0; i < puzzleSprites.Length; i++)
        {
            GameObject obj = Instantiate(piecePrefab, boardParent);
            PuzzlePiece piece = obj.GetComponent<PuzzlePiece>();
            piece.Setup(puzzleSprites[i], i);
            pieces.Add(piece);
        }
    }

    void ShuffleBoard()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int randomIndex = Random.Range(i, pieces.Count);

            int siblingIndexI = pieces[i].transform.GetSiblingIndex();
            int siblingIndexRandom = pieces[randomIndex].transform.GetSiblingIndex();

            pieces[i].transform.SetSiblingIndex(siblingIndexRandom);
            pieces[randomIndex].transform.SetSiblingIndex(siblingIndexI);

            int tempIndex = pieces[i].currentIndex;
            pieces[i].currentIndex = pieces[randomIndex].currentIndex;
            pieces[randomIndex].currentIndex = tempIndex;
        }
    }

    // Novo método para resetar o board sem embaralhar
    void ResetBoardWithoutShuffle()
    {
        CreateBoard();
        commandStack.Clear();
        selectedPiece = null;
    }

    public void OnPieceClicked(PuzzlePiece piece)
    {
        if (selectedPiece == null)
        {
            selectedPiece = piece;
        }
        else
        {
            if (piece != selectedPiece)
            {
                ICommand cmd = new SwapPiecesCommand(selectedPiece, piece);
                cmd.Do();
                commandStack.Push(cmd);
                replayList.Add(cmd); // Registra para replay

                if (IsPuzzleComplete())
                {
                    ShowVictoryScreen();
                }
            }
            selectedPiece = null;
        }
    }

    public void UndoLastMove()
    {
        if (commandStack.Count > 0)
        {
            ICommand cmd = commandStack.Pop();
            cmd.Undo();
        }
    }

    public bool IsPuzzleComplete()
    {
        foreach (var piece in pieces)
        {
            if (piece.currentIndex != piece.correctIndex)
                return false;
        }
        return true;
    }

    public void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        victoryPanel.SetActive(false);
        CreateBoard();
        ShuffleBoard();
        commandStack.Clear();
        replayList.Clear();
        selectedPiece = null;
    }

    public void StartReplay()
    {
        StartCoroutine(ReplayCoroutine());
    }

    IEnumerator ReplayCoroutine()
    {
        victoryPanel.SetActive(false);

        // Reseta o board SEM embaralhar para manter o estado inicial correto
        ResetBoardWithoutShuffle();

        yield return new WaitForSeconds(0.5f);

        foreach (var cmd in replayList)
        {
            cmd.Do();
            yield return new WaitForSeconds(0.5f);
        }

        if (IsPuzzleComplete())
        {
            ShowVictoryScreen();
        }
    }

    public void SkipReplay()
    {
        StopAllCoroutines();

        foreach (var cmd in replayList)
        {
            cmd.Do();
        }

        if (IsPuzzleComplete())
        {
            ShowVictoryScreen();
        }
    }
}
