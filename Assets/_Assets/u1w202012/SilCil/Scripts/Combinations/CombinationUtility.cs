using System;
using System.Collections.Generic;

namespace Unity1Week202012
{
    public static class CombinationUtility
    {
        public static HashSet<PieceData> GetGroup(PieceData pieceData, Func<PieceData, bool> filter)
        {
            HashSet<PieceData> group = new HashSet<PieceData>();

            Queue<PieceData> pieces = new Queue<PieceData>();
            pieces.Enqueue(pieceData);
            while (pieces.Count != 0)
            {
                var piece = pieces.Dequeue();
                if (!filter.Invoke(piece)) continue;

                group.Add(piece);
                
                var neighbors = Services.PieceConnection.GetNeighbors(piece);
                if (neighbors == null) continue;

                foreach (var neighbor in neighbors)
                {
                    if (neighbor == null) continue;
                    if (group.Contains(neighbor)) continue;
                    pieces.Enqueue(neighbor);
                }
            }

            return group;
        }
    }
}