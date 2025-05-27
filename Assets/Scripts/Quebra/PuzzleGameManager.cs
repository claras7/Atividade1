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

    public GameObject victoryPanel;          // Painel de vitória com "Jogar novamente" e "Ver replay"
    public GameObject cancelReplayButton;    // Botão para cancelar o replay (coloque no Canvas, canto superior direito por exemplo)

    public List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    private PuzzlePiece selectedPiece = null;
    private Stack<ICommand> commandStack = new Stack<ICommand>();
    private List<ICommand> replayList = new List<ICommand>();

    private bool isReplaying = false;

    // Guarda o estado inicial das posições das peças após o embaralhamento
    private List<int> initialPositions = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateBoard();
        ShuffleBoard();

        victoryPanel.SetActive(false);
        cancelReplayButton.SetActive(false); // Oculto no início
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

        // Salva o estado inicial após embaralhar
        initialPositions.Clear();
        foreach (var piece in pieces)
        {
            initialPositions.Add(piece.currentIndex);
        }
    }

    // Restaura as posições das peças ao estado inicial salvo (após embaralhar)
    void RestoreInitialPositions()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].currentIndex = initialPositions[i];
            pieces[i].transform.SetSiblingIndex(initialPositions[i]);
        }
    }

    // Usado no replay para restaurar o estado inicial, não recria as peças
    void ResetBoardWithoutShuffle()
    {
        RestoreInitialPositions();
        commandStack.Clear();
        selectedPiece = null;
    }

    public void OnPieceClicked(PuzzlePiece piece)
    {
        if (isReplaying) return;

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
                replayList.Add(cmd);

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
        if (isReplaying) return;

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
        cancelReplayButton.SetActive(false);
        CreateBoard();
        ShuffleBoard();
        commandStack.Clear();
        replayList.Clear();
        selectedPiece = null;
        isReplaying = false;
    }

    public void StartReplay()
    {
        if (isReplaying) return;

        isReplaying = true;
        cancelReplayButton.SetActive(true); // Mostrar botão cancelar replay

        StartCoroutine(ReplayCoroutine());
    }

    IEnumerator ReplayCoroutine()
    {
        victoryPanel.SetActive(false);

        // Restaurar estado inicial antes do replay
        RestoreInitialPositions();

        yield return new WaitForSeconds(0.5f);

        foreach (var cmd in replayList)
        {
            cmd.Do();
            yield return new WaitForSeconds(1f); // espera 1 segundo entre comandos
        }

        cancelReplayButton.SetActive(false); // Oculta botão após replay
        isReplaying = false;

        if (IsPuzzleComplete())
        {
            ShowVictoryScreen();
        }
    }

    public void SkipReplay()
    {
        if (!isReplaying) return;

        StopAllCoroutines();

        // Restaurar estado inicial e aplicar todos os comandos imediatamente
        RestoreInitialPositions();

        foreach (var cmd in replayList)
        {
            cmd.Do();
        }

        cancelReplayButton.SetActive(false);
        isReplaying = false;

        if (IsPuzzleComplete())
        {
            ShowVictoryScreen();
        }
    }
}
