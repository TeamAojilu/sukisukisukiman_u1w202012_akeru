﻿using SilCilSystem.Variables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PiecePlacement))]
    [RequireComponent(typeof(PieceRespawner))]
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        private Camera m_camera = default;
        private PiecePlacement m_piecePlacement = default;
        private PieceRespawner m_pieceRespawner = default;

        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        private void Start()
        {
            m_camera = Camera.main;
            m_piecePlacement = GetComponent<PiecePlacement>();
            m_pieceRespawner = GetComponent<PieceRespawner>();
        }

        private void Update()
        {
            if (!m_isPlaying) return;

            if (m_holding)
            {
                HoldingUpdate();
            }

            if (m_holding == false && Services.PointerInput.PointerDown())
            {
                TryHoldPiece();
            }
            else if(m_holding == true && Services.PointerInput.PointerUp())
            {
                if(Services.PointerInput.Flick(out Vector2 speed))
                {
                    TrashPiece(speed);
                }
                else
                {
                    UnHoldPiece();
                }
            }
        }

        private void HoldingUpdate()
        {
            var mousePos = GetMouseWorldPosition();
            m_holdingPiece.CashedTransform.position = mousePos + m_offset;
        }

        private void UnHoldPiece()
        {
            Vector2Int[] positions = null;
            bool canPlace = false;
            foreach((var origin, var p) in GetUnHoldPositions())
            {
                if (Services.Board.CanPlace(p))
                {
                    Services.PiecePosition.SetPiecePosition(m_holdingPiece, origin);
                    m_onPiecePlaced?.Publish();
                    positions = p;
                    canPlace = true;
                    break;
                }
            }

            if (!canPlace)
            {
                // おけない場合は掴んだ位置に戻す.
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
            }

            m_piecePlacement.PlacePiece(m_holdingPiece, positions);
            m_pieceRespawner.DisablePlaces = null;
            
            m_holding.Value = false;
            m_holdingPiece = null;
        }

        private void TryHoldPiece()
        {
            var mousePos = GetMouseWorldPosition();
            var hit = Physics2D.OverlapPoint(mousePos, m_pieceLayer.value);
            if (hit == null) return;
            if (!hit.TryGetComponent(out Piece piece)) return;

            // つかむ処理.
            m_holding.Value = true;
            m_holdingPiece = piece;
            m_offset = m_holdingPiece.CashedTransform.position - mousePos;
            m_startPosition = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            m_piecePlacement.RemovePiece(m_holdingPiece);
            m_pieceRespawner.DisablePlaces = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
        }

        private void TrashPiece(Vector2 speed)
        {
            Debug.Log($"Flick: {speed}");
            m_piecePlacement.TrashPiece(m_holdingPiece, speed);
            m_onPieceTrashed?.Publish();
            m_pieceRespawner.PopScheduled(piece => m_piecePlacement.PlacePiece(piece));
            m_pieceRespawner.DisablePlaces = null;
            m_holding.Value = false;
            m_holdingPiece = null;
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            return m_camera.ScreenToWorldPoint(Input.mousePosition);
        }

        private IEnumerable<(Vector2Int, Vector2Int[])> GetUnHoldPositions()
        {
            var origin = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            foreach(var o in GetAround(origin))
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