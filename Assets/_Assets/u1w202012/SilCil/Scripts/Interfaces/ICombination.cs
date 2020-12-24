namespace Unity1Week202012
{
    public interface ICombination
    {
        string Evaluate(PieceData pieceData);
        void SetupBeforeEvaluate();
    }
}