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
            // Nota: Esta implementação original pode precisar de ajustes para sempre
            // encontrar a peça correta se os currentIndex mudaram fora de ordem linear.
            // A correção mais robusta geralmente encontra a peça pelo seu current/correctIndex.
            // Para manter a solicitação de "não mexer o resto", mantemos esta versão.
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

        StopAllCoroutines(); // Para a rotina de replay que possa estar em execução

        Debug.Log("SkipReplay iniciado. Forçando o puzzle para o estado completo.");

        // -------------------------------------------------------------------
        // MUDANÇA EXCLUSIVA AQUI: Coloca cada peça diretamente em sua posição final
        // -------------------------------------------------------------------
        foreach (var piece in pieces)
        {
            // Define o índice lógico da peça para ser o seu índice correto
            piece.currentIndex = piece.correctIndex;
            // Define a posição visual da peça (na hierarquia do Transform) para ser o seu índice correto
            // Isso fará com que o GridLayoutGroup (se estiver usando) organize as peças corretamente
            piece.transform.SetSiblingIndex(piece.correctIndex);

            // Adicionado para depuração: Verifique esses valores no Console da Unity
            Debug.Log($"Peça: {piece.name}, Current Index: {piece.currentIndex}, Correct Index: {piece.correctIndex}, Sibling Index: {piece.transform.GetSiblingIndex()}");
        }

        // Limpa as listas de comandos e replay para consistência do estado do jogo.
        commandStack.Clear();
        replayList.Clear();

        cancelReplayButton.SetActive(false); // Oculta o botão de cancelar
        isReplaying = false; // Define que o replay não está mais ativo

        // Verifica se o puzzle está completo (deve ser TRUE agora) e exibe a tela de vitória
        if (IsPuzzleComplete())
        {
            Debug.Log("Puzzle está completo após SkipReplay. Exibindo tela de vitória.");
            ShowVictoryScreen();
        }
        else
        {
            // Isso indica que IsPuzzleComplete() não retornou true, mesmo após o SkipReplay.
            // Possíveis causas: 'currentIndex' ou 'correctIndex' não estão como esperado.
            Debug.LogError("Puzzle NÃO está completo após SkipReplay. Houve um erro no posicionamento das peças ou na verificação.");
        }
    }
}
