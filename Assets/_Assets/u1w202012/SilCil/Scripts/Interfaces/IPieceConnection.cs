using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IPieceConnection
    {
        void Add(Piece piece);
        void Clear();
        void Remove(Piece piece);
        IEnumerable<Piece> GetNeighbors(Piece piece);
    }
}