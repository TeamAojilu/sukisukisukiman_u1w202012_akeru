using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SampleInitialPieceGenerator : IInitialPieceGenerator
    {
        private PieceData[] m_candidates = new PieceData[]
        {
            PieceData.Create(Constants.ColorNames[0], Constants.ShapeNames[0], new Vector2Int[]
            {
                Vector2Int.zero,
                Vector2Int.right,
                Vector2Int.down,
            }),
            PieceData.Create(Constants.ColorNames[0], Constants.ShapeNames[1], new Vector2Int[]
            {
                Vector2Int.zero,
                Vector2Int.right,
                Vector2Int.left,
            }),
        };

        private Vector2Int[] m_origins = new Vector2Int[]
        {
            new Vector2Int(-2, 3),
            new Vector2Int(2, 3),
            new Vector2Int(2, -1),
            new Vector2Int(-2, -1),
        };

        public IEnumerable<Piece> CreateInitialPieces()
        {
            foreach (var origin in m_origins)
            {
                var type = m_candidates[Random.Range(0, m_candidates.Length)];
                var piece = Services.PieceObjectFactory.Create(type);
                Services.PiecePosition.SetPiecePosition(piece, origin);
                yield return piece;
            }
        }
    }
}