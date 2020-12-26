using SilCilSystem.Variables;
using UnityEngine;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PieceHolder))]
    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEventListener m_onSubmit = default;
        
        private PieceHolder m_pieceHolder = default;

        private void Start()
        {
            m_pieceHolder = GetComponent<PieceHolder>();
            m_onSubmit?.Subscribe(OnSubmit).DisposeOnDestroy(gameObject);
        }

        private void Update()
        {
            if (!m_isPlaying) return;

            if (m_holding)
            {
                m_pieceHolder.HoldingUpdate(Services.PointerInput.PointerWorldPosition);
            }

            if (m_holding == false && Services.PointerInput.PointerDown())
            {
                m_pieceHolder.TryHoldPiece(Services.PointerInput.PointerWorldPosition);
            }
            else if(m_holding == true && Services.PointerInput.PointerUp())
            {
                if(Services.PointerInput.Flick(out Vector2 speed))
                {
                    m_pieceHolder.TrashPiece(speed);
                }
                else
                {
                    m_pieceHolder.UnHoldPiece();
                }
            }
        }

        private void OnSubmit()
        {
            if (m_holding)
            {
                m_pieceHolder.UnHoldPiece(backStartPosition: true);
            }
        }
    }
}