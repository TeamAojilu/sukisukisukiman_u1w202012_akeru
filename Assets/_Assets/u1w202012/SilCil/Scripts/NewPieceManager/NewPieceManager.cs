using SilCilSystem.Math;
using SilCilSystem.Variables;
using System.Collections;
using UnityEngine;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PieceRespawner))]
    public class NewPieceManager : MonoBehaviour
    {
        [Header("Playing")]
        [SerializeField] private VariableBool m_isPlaying = default;
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;
        [SerializeField] private GameEventListener m_onSubmit = default;

        [Header("Holder")]
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private GameEvent m_onPieceMoveCancelled = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        [Header("Score")]
        [SerializeField] private VariableInt m_score = default;
        [SerializeField] private VariableInt m_estimatedScore = default;
        [SerializeField] private FloatToInt.CastType m_floatToInt = default;
        [SerializeField] private GameEvent m_onEvalulated = default;

        private NewPieceHolder m_pieceHolder = default;
        private NewPiecePlacement m_piecePlacement = default;
        private PieceRespawner m_pieceRespawner = default;

        private bool m_isSubmitting = false;
        
        private void Start()
        {
            m_pieceRespawner = GetComponent<PieceRespawner>();
            m_piecePlacement = new NewPiecePlacement();
            m_pieceHolder = new NewPieceHolder(m_holding, m_pieceLayer.value, m_pieceRespawner, m_piecePlacement, Camera.main);
            
            m_onSubmit?.Subscribe(OnSubmit).DisposeOnDestroy(gameObject);
            m_onIsPlayingChanged?.Subscribe(x => { if (x) PlayNew(); }).DisposeOnDestroy(gameObject);
        }

        private void PlayNew()
        {
            CancelMovingPiece();
            m_piecePlacement.CreateInitialPieces();
            UpdateEstimatedScore();
        }
        
        private void UpdateEstimatedScore()
        {
            double score = Services.ScoreCalculator?.Evaluate(m_piecePlacement.GetPieces()) ?? 0.0;
            score = Services.BonusCalculator?.Evaluate(score, m_piecePlacement.GetPieces()) ?? score;
            m_estimatedScore.Value = m_floatToInt.Cast((float) score);
        }
        
        private void Update()
        {
            if (!m_isPlaying) return;
            if (m_isSubmitting) return;

            if (m_holding)
            {
                m_pieceHolder.HoldingUpdate(Services.PointerInput.PointerWorldPosition);
                UpdateEstimatedScore();
            }

            if (m_holding == false && Services.PointerInput.PointerDown())
            {
                m_pieceHolder.HoldPiece(Services.PointerInput.PointerWorldPosition);
                UpdateEstimatedScore();
            }
            else if(m_holding == true && Services.PointerInput.PointerUp())
            {
                if(Services.PointerInput.Flick(out Vector2 speed))
                {
                    m_pieceHolder.TrashPiece(speed);
                    m_onPieceTrashed?.Publish();
                }
                else if(m_pieceHolder.UnHoldPiece())
                {
                    m_onPiecePlaced?.Publish();
                }
                else
                {
                    m_onPieceMoveCancelled?.Publish();
                }
                UpdateEstimatedScore();
            }
        }

        private void OnSubmit()
        {
            if (!m_isPlaying) return;
            if (m_isSubmitting) return;
            StartCoroutine(SubmitCoroutine());
        }

        private IEnumerator SubmitCoroutine()
        {
            m_isSubmitting = true;
            CancelMovingPiece();
            UpdateEstimatedScore();

            m_score.Value = m_estimatedScore;
            yield return Services.BonusEffect?.BonusEffectCoroutine();

            m_onEvalulated?.Publish();
            m_isSubmitting = false;
            m_isPlaying.Value = false;
        }

        private void CancelMovingPiece()
        {
            if (m_holding)
            {
                m_pieceHolder.UnHoldPiece(backStartPosition: true);
                m_onPieceMoveCancelled?.Publish();
            }
        }
    }
}