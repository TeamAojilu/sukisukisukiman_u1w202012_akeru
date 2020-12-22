using UnityEngine;

namespace Unity1Week202012
{
    public class Piece : MonoBehaviour
    {
        public Vector2Int[] m_positions = default;

        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;
    }
}