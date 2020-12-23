using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public interface IBoard
    {
        bool CanPlace(IEnumerable<Vector2Int> positions);
        void Place(IEnumerable<Vector2Int> positions);
        void Remove(IEnumerable<Vector2Int> positions);
    }
}