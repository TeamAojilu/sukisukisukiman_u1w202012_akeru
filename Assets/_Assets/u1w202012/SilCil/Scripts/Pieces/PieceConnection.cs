using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class PieceConnection : IPieceConnection
    {
        private List<PieceData> m_pieces = new List<PieceData>();
        private Dictionary<PieceData, List<PieceData>> m_neighbors = new Dictionary<PieceData, List<PieceData>>();

        public void Add(PieceData piece)
        {
            if (m_pieces.Contains(piece)) return;

            m_pieces.Add(piece);
            m_neighbors[piece] = CreateNeighborList(piece);
            foreach(var neighbor in m_neighbors[piece])
            {
                m_neighbors[neighbor].Add(piece);
            }
        }
        
        public void Remove(PieceData piece)
        {
            if (!m_pieces.Contains(piece)) return;
            
            foreach(var neighbor in m_neighbors[piece])
            {
                m_neighbors[neighbor].Remove(piece);
            }
            m_neighbors.Remove(piece);
            m_pieces.Remove(piece);
        }

        public void Clear()
        {
            m_pieces.Clear();
            m_neighbors.Clear();
        }
        
        public IEnumerable<PieceData> GetNeighbors(PieceData piece)
        {
            return m_neighbors[piece];
        }

        private List<PieceData> CreateNeighborList(PieceData pieceData)
        {
            List<PieceData> neighbors = new List<PieceData>();
            foreach (var p in m_pieces)
            {
                if (p == pieceData) continue;
                if (!IsContact(p, pieceData)) continue;
                neighbors.Add(p);
            }
            return neighbors;
        }

        private bool IsContact(PieceData piece1, PieceData piece2)
        {
            foreach (var pos in piece1.m_positions)
            {
                if (piece2.m_positions.Any(p => IsNeighbor(p, pos))) return true;
            }
            return false;
        }

        private bool IsNeighbor(Vector2Int p1, Vector2Int p2)
        {
            return Mathf.Abs(p1.x - p2.x) == 1 && Mathf.Abs(p1.y - p2.y) == 1;
        }
    }
}