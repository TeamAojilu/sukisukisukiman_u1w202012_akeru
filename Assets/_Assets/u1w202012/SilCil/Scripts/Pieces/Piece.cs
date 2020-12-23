using UnityEngine;

namespace Unity1Week202012
{
    [System.Serializable]
    public class PieceData
    {
        public string m_type1 = "red";
        public string m_type2 = "0";
        public Vector2Int[] m_positions = default;

        private PieceData(string type1, string type2, Vector2Int[] positions)
        {
            m_type1 = type1;
            m_type2 = type2;
            m_positions = positions;
        }

        public static PieceData Create(string type1, string type2, Vector2Int[] positions)
        {
            return new PieceData(type1, type2, positions);
        }

        /// <summary>typeは同じ, positionsは違うものをnewして作成</summary>
        public static PieceData Create(PieceData piece, Vector2Int[] positions)
        {
            return new PieceData(piece.m_type1, piece.m_type2, positions);
        }
    }

    public class Piece : MonoBehaviour
    {
        public PieceData m_pieceData = default;

        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;
    }
}