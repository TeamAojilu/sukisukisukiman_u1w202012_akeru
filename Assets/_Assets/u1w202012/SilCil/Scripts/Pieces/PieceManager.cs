using SilCilSystem.Variables;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PiecePlacement))]
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        private Camera m_camera = default;
        private PiecePlacement m_piecePlacement = default;

        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        private void Start()
        {
            m_camera = Camera.main;
            m_piecePlacement = GetComponent<PiecePlacement>();
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
                // おけない場合は掴んだ位置に戻す.
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
            }

            m_piecePlacement.PlacePiece(m_holdingPiece, positions);
            
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
        }

        private void TrashPiece(Vector2 speed)
        {
            Debug.Log($"Flick: {speed}");
            m_piecePlacement.TrashPiece(m_holdingPiece, speed);
            m_onPieceTrashed?.Publish();
            m_holding.Value = false;
            m_holdingPiece = null;
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            return m_camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}