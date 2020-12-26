using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class NewPiecePlacement
    {
        private Dictionary<Piece, PieceData> m_pieceData = new Dictionary<Piece, PieceData>();
        
        public IEnumerable<PieceData> GetPieces()
        {
            return m_pieceData.Values;
        }

        public void CreateInitialPieces()
        {
            // 盤面を消去.
            foreach (var piece in m_pieceData.Keys)
            {
                if (piece == null) continue;
                if (piece.gameObject == null) continue;
                GameObject.Destroy(piece.gameObject);
            }

            Services.Board.Clear();
            Services.PieceConnection.Clear();
            m_pieceData.Clear();

            // 新しく生成.
            foreach (var piece in Services.InitialPieceGenerator.CreateInitialPieces())
            {
                PlacePiece(piece);
            }
        }

        public void PlacePiece(Piece piece)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            PlacePiece(piece, piece.m_pieceData.m_positions.Select(x => x + origin).ToArray());
        }

        public void PlacePiece(Piece piece, Vector2Int[] positions)
        {
            PieceData pieceData = GetPieceData(piece, positions);
            Services.PieceConnection.Add(pieceData);
            Services.Board.Place(positions);
        }

        public void RemovePiece(Piece piece)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            Services.Board.Remove(Services.PiecePosition.GetBlockPositions(piece, origin));

            if (m_pieceData.ContainsKey(piece))
            {
                Services.PieceConnection.Remove(m_pieceData[piece]);
            }
        }

        public void TrashPiece(Piece piece, Vector2 speed)
        {
            RemovePiece(piece);
            if (m_pieceData.ContainsKey(piece)) m_pieceData.Remove(piece);

            // とりあえず削除で.
            GameObject.Destroy(piece.gameObject);
        }
        
        private PieceData GetPieceData(Piece piece, Vector2Int[] positions)
        {
            if (m_pieceData.ContainsKey(piece))
            {
                var data = m_pieceData[piece];
                data.m_positions = positions;
            }
            else
            {
                var data = PieceData.Create(piece.m_pieceData, positions);
                m_pieceData.Add(piece, data);
            }
            return m_pieceData[piece];
        }
    }
}