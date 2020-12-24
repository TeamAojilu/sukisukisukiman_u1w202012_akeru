using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IInitialPieceGenerator
    {
        IEnumerable<Piece> CreateInitialPieces();
    }
}