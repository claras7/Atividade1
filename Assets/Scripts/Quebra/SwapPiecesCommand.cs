public class SwapPiecesCommand : ICommand
{
    private PuzzlePiece pieceA, pieceB;

    public SwapPiecesCommand(PuzzlePiece a, PuzzlePiece b)
    {
        pieceA = a;
        pieceB = b;
    }

    public void Do()
    {
        Swap(pieceA, pieceB);
    }

    public void Undo()
    {
        Swap(pieceA, pieceB);
    }

    private void Swap(PuzzlePiece a, PuzzlePiece b)
    {
        int siblingA = a.transform.GetSiblingIndex();
        int siblingB = b.transform.GetSiblingIndex();

        a.transform.SetSiblingIndex(siblingB);
        b.transform.SetSiblingIndex(siblingA);

        int tempIndex = a.currentIndex;
        a.currentIndex = b.currentIndex;
        b.currentIndex = tempIndex;
    }
}


    