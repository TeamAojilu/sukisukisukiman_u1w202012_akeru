using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class RectangleBoard : MonoBehaviour, IBoard, ISpaceChecker
    {
        [SerializeField] private Vector2Int m_rangeX = new Vector2Int(-3, 4);
        [SerializeField] private Vector2Int m_rangeY = new Vector2Int(-3, 4);

        private Vector2Int m_offset = default;
        private bool[,] m_filled = default;

        private void Start()
        {
            m_offset = -new Vector2Int(m_rangeX.x, m_rangeY.x);
            m_filled = new bool[m_rangeX.y - m_rangeX.x + 1, m_rangeY.y - m_rangeY.x + 1];
            Clear();
            Services.Board = this;
            Services.SpaceChecker = this;
        }

        #region IBoardの実装
        public bool CanPlace(IEnumerable<Vector2Int> positions)
        {
            return positions.All(p => !IsFilled(p));
        }

        public void Place(IEnumerable<Vector2Int> positions) => SetFilled(positions, true);

        public void Remove(IEnumerable<Vector2Int> positions) => SetFilled(positions, false);

        private void SetFilled(IEnumerable<Vector2Int> positions, bool filled)
        {
            foreach (var pos in positions)
            {
                var index = ToIndex(pos);
                if (!IsInRange(index.x, index.y)) continue;
                m_filled[index.x, index.y] = filled;
            }
        }

        private bool IsFilled(Vector2Int position)
        {
            Vector2Int index = ToIndex(position);
            if (!IsInRange(index.x, index.y)) return true;
            return m_filled[index.x, index.y];
        }

        private bool IsInRange(int x, int y)
        {
            if (x < 0 || x >= m_filled.GetLength(0)) return false;
            if (y < 0 || y >= m_filled.GetLength(1)) return false;
            return true;
        }

        private Vector2Int ToIndex(Vector2Int position)
        {
            return position + m_offset;
        }

        public void Clear()
        {
            for(int x = 0; x < m_filled.GetLength(0); x++)
            {
                for(int y = 0; y < m_filled.GetLength(1); y++)
                {
                    m_filled[x, y] = false;
                }
            }
        }
        #endregion

        #region ISpaceChecker
        public IEnumerable<IEnumerable<Vector2Int>> GetIsolatedSpaces()
        {
            HashSet<Vector2Int> hasChecked = new HashSet<Vector2Int>();
            
            for (int x = m_rangeX.x; x <= m_rangeX.y; x++)
            {
                for(int y = m_rangeY.x; y <= m_rangeY.y; y++)
                {
                    var position = new Vector2Int(x, y);
                    if (IsFilled(position)) continue;
                    if (hasChecked.Contains(position)) continue;
                    List<Vector2Int> group = GetGroup(position);
                    hasChecked.UnionWith(group);
                    yield return group;
                }
            }
        }

        private List<Vector2Int> GetGroup(Vector2Int position)
        {
            List<Vector2Int> group = new List<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(position);
            while (queue.Count != 0)
            {
                var pos = queue.Dequeue();
                if (group.Contains(pos)) continue;
                group.Add(pos);
                foreach (var next in GetNeighbors(pos))
                {
                    if (IsFilled(next)) continue;
                    queue.Enqueue(next);
                }
            }

            return group;
        }

        private IEnumerable<Vector2Int> GetNeighbors(Vector2Int position)
        {
            yield return position + Vector2Int.up;
            yield return position + Vector2Int.right;
            yield return position + Vector2Int.down;
            yield return position + Vector2Int.left;
        }
        #endregion
    }
}