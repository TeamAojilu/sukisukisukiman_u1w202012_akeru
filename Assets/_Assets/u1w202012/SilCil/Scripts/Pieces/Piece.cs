using UnityEngine;

namespace Unity1Week202012
{
    [System.Serializable]
    public class PieceData
    {
        public string m_type1 = "red";
        public string m_type2 = "0";
        public Vector2Int[] m_positions = default;
    }

    public class Piece : MonoBehaviour
    {
        public PieceData m_pieceData = default;

        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;
    }
}