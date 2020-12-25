using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SampleBoard : IBoard
    {
        private HashSet<Vector2Int> m_filled = new HashSet<Vector2Int>();

        public bool CanPlace(IEnumerable<Vector2Int> positions)
        {
            return positions.All(pos => m_filled.Contains(pos) == false);
        }

        public void Clear()
        {
            m_filled.Clear();
        }

        public void Place(IEnumerable<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                if (m_filled.Contains(pos)) continue;
                m_filled.Add(pos);
            }
        }

        public void Remove(IEnumerable<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                m_filled.Remove(pos);
            }
        }
    }
}