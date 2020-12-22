using SilCilSystem.Variables;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
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

            if (m_holding == false && Services.PointerInput.PointerDown())
            {
                TryHoldPiece();
            }
            else if(m_holding == true && Services.PointerInput.PointerUp())
            {
                if(Services.PointerInput.Flick(out Vector2 speed))
                {
                    print($"Flick: {speed}");
                    m_onPieceTrashed?.Publish();
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
            var origin = Services.PiecePosition.GetOriginPosition(m_holdingPiece);

            // 複数候補で検索したほうが操作性が良くなるかもしれないが、とりあえず一つでやる.
            var positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, origin).ToArray();
            if (Services.Board.CanPlace(positions))
            {
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, origin);
                m_onPiecePlaced?.Publish();
            }
            else
            {
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
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
            m_startPosition = Services.PiecePosition.GetOriginPosition(m_holdingPiece);
            Services.Board.Remove(Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition));
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            return m_camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}