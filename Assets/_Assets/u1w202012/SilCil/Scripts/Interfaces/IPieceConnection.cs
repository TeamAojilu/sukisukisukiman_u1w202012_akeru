using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IPieceConnection
    {
        void Add(PieceData piece);
        void Clear();
        void Remove(PieceData piece);
        IEnumerable<PieceData> GetNeighbors(PieceData piece);
    }
}