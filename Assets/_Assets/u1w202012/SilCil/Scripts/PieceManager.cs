using SilCilSystem.Variables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        private Camera m_camera = default;
        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        private void Start()
        {
            m_camera = Camera.main;
        }

        private void Update()
        {
            if (!m_isPlaying) return;

            if (m_holding)
            {
                HoldingUpdate();
            }

            if (m_holding == false && Input.GetMouseButtonDown(0))
            {
                TryHoldPiece();
            }
            else if(m_holding == true && Input.GetMouseButtonUp(0))
            {
                UnHoldPiece();
            }
        }

        private void HoldingUpdate()
        {
            var mousePos = GetMouseWorldPosition();
            m_holdingPiece.CashedTransform.position = mousePos + m_offset;
        }

        private void UnHoldPiece()
        {
            var origin = GetOriginPosition(m_holdingPiece);

            // 複数候補で検索したほうが操作性が良くなるかもしれないが、とりあえず一つでやる.
            var positions = GetHoldingPiecePositions(origin).ToArray();
            if (Services.Board.CanPlace(positions))
            {
                SetPiecePosition(m_holdingPiece, origin);
            }
            else
            {
                SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = GetHoldingPiecePositions(m_startPosition).ToArray();
            }

            m_holding.Value = false;
            m_holdingPiece = null;
            Services.Board.Place(positions);
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
            m_startPosition = GetOriginPosition(m_holdingPiece);
            Services.Board.Remove(GetHoldingPiecePositions(m_startPosition));
        }

        private IEnumerable<Vector2Int> GetHoldingPiecePositions(Vector2Int origin)
        {
            return m_holdingPiece.m_positions.Select(p => p + origin);
        }

        private Vector2Int GetOriginPosition(Piece piece)
        {
            var pos = m_holdingPiece.CashedTransform.position;
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)); // これは操作性に関わる項目なので要調整.
        }

        private void SetPiecePosition(Piece piece, Vector2Int pos)
        {
            piece.CashedTransform.position = new Vector3(pos.x, pos.y, 0f);
        }

        private Vector3 GetMouseWorldPosition()
        {
            return m_camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}