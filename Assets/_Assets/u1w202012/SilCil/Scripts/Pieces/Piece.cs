using UnityEngine;

namespace Unity1Week202012
{
    [System.Serializable]
    public class PieceData
    {
        public string m_color = "red";
        public string m_shape = "0";
        public Vector2Int[] m_positions = default;

        private PieceData(string color, string shape, Vector2Int[] positions)
        {
            m_color = color;
            m_shape = shape;
            m_positions = positions;
        }

        public static PieceData Create(string type1, string type2, Vector2Int[] positions)
        {
            return new PieceData(type1, type2, positions);
        }

        /// <summary>typeは同じ, positionsは違うものをnewして作成</summary>
        public static PieceData Create(PieceData piece, Vector2Int[] positions)
        {
            return new PieceData(piece.m_color, piece.m_shape, positions);
        }
    }

    public class Piece : MonoBehaviour
    {
        public PieceData m_pieceData = default;

        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;
    }
}