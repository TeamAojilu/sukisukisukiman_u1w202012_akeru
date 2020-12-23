namespace Unity1Week202012
{
    public interface IPieceObjectFactory
    {
        Piece Create(PieceData pieceData);
    }
}