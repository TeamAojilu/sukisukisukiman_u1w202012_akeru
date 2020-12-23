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
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        private Camera m_camera = default;
        private Piece m_holdingPiece = default;
        private Vector3 m_offset = default;
        private Vector2Int m_startPosition = default;

        private Dictionary<Piece, PieceData> m_pieceData = new Dictionary<Piece, PieceData>();

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
                // おけない場合は掴んだ位置に戻す.
                Services.PiecePosition.SetPiecePosition(m_holdingPiece, m_startPosition);
                positions = Services.PiecePosition.GetBlockPositions(m_holdingPiece, m_startPosition).ToArray();
            }

            PieceData pieceData = GetPieceData(m_holdingPiece, positions);
            Services.PieceConnection.Add(pieceData);
            Services.Board.Place(positions);
            m_holding.Value = false;
            m_holdingPiece = null;
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
                m_pieceData.Add(m_holdingPiece, data);
            }

            return m_pieceData[piece];
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

            if (m_pieceData.ContainsKey(m_holdingPiece))
            {
                Services.PieceConnection.Remove(m_pieceData[m_holdingPiece]);
            }
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            return m_camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}