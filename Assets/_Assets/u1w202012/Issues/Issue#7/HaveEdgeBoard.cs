using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class HaveEdgeBoard : MonoBehaviour, IBoard
    {
        [SerializeField] private Vector2 m_centerPosition = Vector2.zero;
        [SerializeField] private Vector2Int m_BoardRangeFromCenter_max = new Vector2Int(3, 3);
        [SerializeField]private Vector2Int m_BoardRangeFromCenter_min = new Vector2Int(-4, -4);

        private HashSet<Vector2Int> m_filled = new HashSet<Vector2Int>();

        private void Start()
        {
            Services.Board = this;
        }

        public bool CanPlace(IEnumerable<Vector2Int> positions)
        {
            bool isBlank= positions.All(pos => m_filled.Contains(pos) == false);

            Vector2 boardRangeMin = m_BoardRangeFromCenter_min + m_centerPosition;
            Vector2 boardRangeMax = m_BoardRangeFromCenter_max + m_centerPosition;
            bool isInArea = positions.All(pos =>
              boardRangeMin.x<= pos.x && pos.x <= boardRangeMax.x
              && boardRangeMin.y<=pos.y&& pos.y<=boardRangeMax.y);

            return isBlank && isInArea;
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