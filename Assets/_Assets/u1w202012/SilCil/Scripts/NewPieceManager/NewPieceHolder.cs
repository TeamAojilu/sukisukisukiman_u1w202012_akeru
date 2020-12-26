using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class NewPieceHolder
    {
        private readonly VariableBool m_holding = default;
        private readonly int m_pieceLayer = default;
        private readonly PieceRespawner m_pieceRespawner = default;
        private readonly NewPiecePlacement m_piecePlacement = default;
        private readonly Camera m_camera = default;

        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        public NewPieceHolder(VariableBool holding, int pieceLayer, PieceRespawner pieceRespawner, NewPiecePlacement piecePlacement, Camera camera)
        {
            m_holding = holding;
            m_pieceLayer = pieceLayer;
            m_pieceRespawner = pieceRespawner;
            m_piecePlacement = piecePlacement;
            m_camera = camera;
        }
        
        public void HoldingUpdate(Vector3 mouseWorldPosition)
        {
            m_holdingPiece.CashedTransform.position = mouseWorldPosition + m_offset;
        }

        public bool UnHoldPiece(bool backStartPosition = false)
        {
            if (m_holdingPiece == null) return false;

            Vector2Int[] positions = null;

            // 離した位置が置けるかどうかを判定.
            bool canPlace = false;
            if (!backStartPosition)
            {
                foreach ((var origin, var p) in GetUnHoldPositions())
                {
                    if (Services.Board.CanPlace(p))
                    {
                        Services.PiecePosition.SetPiecePosition(m_holdingPiece, origin);
                        positions = p;
                        canPlace = true;
                        break;
                    }
                }
            }

            // おけない場合は掴んだ位置に戻す.
            if (!canPlace)
            {
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
            }
            PlacePiece(m_holdingPiece, positions);
            return canPlace;
        }

        public bool HoldPiece(Vector3 mouseWorldPosition)
        {
            var hit = Physics2D.OverlapPoint(mouseWorldPosition, m_pieceLayer);
            if (hit == null) return false;
            if (!hit.TryGetComponent(out Piece piece)) return false;

            // つかむ処理.
            m_holding.Value = true;
            m_holdingPiece = piece;
            m_offset = m_holdingPiece.CashedTransform.position - mouseWorldPosition;
            m_startPosition = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            m_piecePlacement.RemovePiece(m_holdingPiece);
            m_pieceRespawner.DisablePlaces = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();

            return true;
        }

        public void TrashPiece(Vector2 speed)
        {
            Debug.Log($"Flick: {speed}");
            m_piecePlacement.TrashPiece(m_holdingPiece, speed);
            SetHoldingFalse();
            m_pieceRespawner.PopScheduled(piece => m_piecePlacement.PlacePiece(piece));
        }

        private void PlacePiece(Piece piece, Vector2Int[] positions)
        {
            m_piecePlacement.PlacePiece(piece, positions);
            SetHoldingFalse();
        }

        private void SetHoldingFalse()
        {
            m_pieceRespawner.DisablePlaces = null;
            m_holding.Value = false;
            m_holdingPiece = null;
        }

        private IEnumerable<(Vector2Int, Vector2Int[])> GetUnHoldPositions()
        {
            var origin = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            foreach (var o in GetAround(origin))
            {
                yield return (o, Services.PiecePosition.GetBlockPositions(m_holdingPiece, o).ToArray());
            }
        }

        private IEnumerable<Vector2Int> GetAround(Vector2Int origin)
        {
            yield return origin + Vector2Int.zero;
            yield return origin + new Vector2Int(0, 1);
            yield return origin + new Vector2Int(1, 1);
            yield return origin + new Vector2Int(1, 0);
            yield return origin + new Vector2Int(1, -1);
            yield return origin + new Vector2Int(0, -1);
            yield return origin + new Vector2Int(-1, -1);
            yield return origin + new Vector2Int(-1, 0);
            yield return origin + new Vector2Int(-1, 1);
        }
    }
}