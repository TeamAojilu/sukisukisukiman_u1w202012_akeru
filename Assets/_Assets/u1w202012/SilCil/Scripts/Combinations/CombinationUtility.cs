using System;
using System.Collections.Generic;

namespace Unity1Week202012
{
    public static class CombinationUtility
    {
        public static HashSet<PieceData> GetGroup(PieceData pieceData, ref HashSet<PieceData> hasChecked, Func<PieceData, bool> filter)
        {
            HashSet<PieceData> group = new HashSet<PieceData>();

            Queue<PieceData> pieces = new Queue<PieceData>();
            pieces.Enqueue(pieceData);
            while (pieces.Count != 0)
            {
                var piece = pieces.Dequeue();
                hasChecked.Add(piece);
                group.Add(piece);
                foreach (var neighbor in Services.PieceConnection.GetNeighbors(piece))
                {
                    if (neighbor == null) continue;
                    if (hasChecked.Contains(neighbor)) continue;
                    if (filter?.Invoke(piece) != true) continue;
                    pieces.Enqueue(neighbor);
                }
            }

            return group;
        }
    }
}