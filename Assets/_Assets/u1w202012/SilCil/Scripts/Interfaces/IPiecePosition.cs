using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public interface IPiecePosition
    {
        IEnumerable<Vector2Int> GetBlockPositions(Piece piece, Vector2Int origin);
        Vector2Int GetOriginPosition(Piece piece);
        void SetPiecePosition(Piece piece, Vector2Int pos);
    }
}