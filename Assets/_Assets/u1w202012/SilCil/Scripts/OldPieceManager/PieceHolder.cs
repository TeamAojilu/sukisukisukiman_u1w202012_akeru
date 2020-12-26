using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PieceRespawner))]
    public class PieceHolder : MonoBehaviour
    {
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        private Camera m_camera = default;
        private IPiecePlacement m_piecePlacement = default;
        private PieceRespawner m_pieceRespawner = default;

        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        private void Start()
        {
            m_camera = Camera.main;
            m_piecePlacement = GetComponent<IPiecePlacement>();
            m_pieceRespawner = GetComponent<PieceRespawner>();
        }

        public void HoldingUpdate(Vector3 mouseWorldPosition)
        {
            m_holdingPiece.CashedTransform.position = mouseWorldPosition + m_offset;
        }

        public void UnHoldPiece(bool backStartPosition = false)
        {
            if (m_holdingPiece == null) return;

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
        }

        public void TryHoldPiece(Vector3 mouseWorldPosition)
        {
            var hit = Physics2D.OverlapPoint(mouseWorldPosition, m_pieceLayer.value);
            if (hit == null) return;
            if (!hit.TryGetComponent(out Piece piece)) return;

            // つかむ処理.
            m_holding.Value = true;
            m_holdingPiece = piece;
            m_offset = m_holdingPiece.CashedTransform.position - mouseWorldPosition;
            m_startPosition = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            m_piecePlacement.RemovePiece(m_holdingPiece);
            m_pieceRespawner.DisablePlaces = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
        }

        public void TrashPiece(Vector2 speed)
        {
            Debug.Log($"Flick: {speed}");
            m_piecePlacement.TrashPiece(m_holdingPiece, speed);
            m_onPieceTrashed?.Publish();
            m_pieceRespawner.DisablePlaces = null;
            m_pieceRespawner.PopScheduled(piece => m_piecePlacement.PlacePiece(piece));
            m_holding.Value = false;
            m_holdingPiece = null;
        }

        private void PlacePiece(Piece piece, Vector2Int[] positions)
        {
            m_piecePlacement.PlacePiece(piece, positions);
            m_pieceRespawner.DisablePlaces = null;

            m_holding.Value = false;
            m_holdingPiece = null;
            m_onPiecePlaced?.Publish();
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