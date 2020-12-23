using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class PieceConnection : IPieceConnection
    {
        private List<Piece> m_pieces = new List<Piece>();
        private Dictionary<Piece, List<Piece>> m_neighbors = new Dictionary<Piece, List<Piece>>();

        public void Add(Piece piece)
        {
            if (m_pieces.Contains(piece)) return;

            m_pieces.Add(piece);
            m_neighbors[piece] = CreateNeighborList(piece);
            foreach(var neighbor in m_neighbors[piece])
            {
                m_neighbors[neighbor].Add(piece);
            }
        }
        
        public void Remove(Piece piece)
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
        
        public IEnumerable<Piece> GetNeighbors(Piece piece)
        {
            return m_neighbors[piece];
        }

        private List<Piece> CreateNeighborList(Piece piece)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            var blocks = Services.PiecePosition.GetBlockPositions(piece, origin).ToArray();

            List<Piece> neighbors = new List<Piece>();
            foreach (var p in m_pieces)
            {
                if (p == piece) continue;
                if (!IsContact(p, blocks)) continue;
                neighbors.Add(p);
            }
            return neighbors;
        }

        private bool IsContact(Piece piece, in Vector2Int[] blocks)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            foreach (var pos in Services.PiecePosition.GetBlockPositions(piece, origin))
            {
                if (blocks.Any(b => IsNeighbor(b, pos))) return true;
            }
            return false;
        }

        private bool IsNeighbor(Vector2Int p1, Vector2Int p2)
        {
            return Mathf.Abs(p1.x - p2.x) == 1 && Mathf.Abs(p1.y - p2.y) == 1;
        }
    }
}