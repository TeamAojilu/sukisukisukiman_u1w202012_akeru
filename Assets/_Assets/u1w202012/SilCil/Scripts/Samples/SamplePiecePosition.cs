using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SamplePiecePosition : IPiecePosition
    {
        public IEnumerable<Vector2Int> GetBlockPositions(Piece piece, Vector2Int origin)
        {
            return piece.m_pieceData.m_positions.Select(p => p + origin);
        }

        public Vector2Int GetOriginPosition(Piece piece)
        {
            var pos = piece.CashedTransform.position;
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        }

        public void SetPiecePosition(Piece piece, Vector2Int pos)
        {
            piece.CashedTransform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }
}