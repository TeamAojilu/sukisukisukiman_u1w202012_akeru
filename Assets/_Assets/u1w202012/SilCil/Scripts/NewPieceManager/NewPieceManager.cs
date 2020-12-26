using SilCilSystem.Math;
using SilCilSystem.Variables;
using UnityEngine;

namespace Unity1Week202012
{
    [RequireComponent(typeof(PieceRespawner))]
    public class NewPieceManager : MonoBehaviour
    {
        [Header("Playing")]
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;
        [SerializeField] private GameEventListener m_onSubmit = default;

        [Header("Holder")]
        [SerializeField] private VariableBool m_holding = default;
        [SerializeField] private GameEvent m_onPiecePlaced = default;
        [SerializeField] private GameEvent m_onPieceTrashed = default;
        [SerializeField] private GameEvent m_onPieceMoveCancelled = default;
        [SerializeField] private LayerMask m_pieceLayer = default;

        [Header("Score")]
        [SerializeField] private VariableInt m_estimatedScore = default;
        [SerializeField] private ReadonlyPropertyFloat m_bonusMultiply = new ReadonlyPropertyFloat(30f);
        [SerializeField] private FloatToInt.CastType m_floatToInt = default;

        private NewPieceHolder m_pieceHolder = default;
        private NewPiecePlacement m_piecePlacement = default;
        private PieceRespawner m_pieceRespawner = default;

        private IScoreCalculator m_scoreCalculator = default;
        private NewBonusScoreCalculator m_bonusScoreCalculator = default;

        private void Start()
        {
            m_pieceRespawner = GetComponent<PieceRespawner>();
            m_piecePlacement = new NewPiecePlacement();
            m_pieceHolder = new NewPieceHolder(m_holding, m_pieceLayer.value, m_pieceRespawner, m_piecePlacement, Camera.main);

            m_scoreCalculator = new NewCombinationScoreCalculator();
            m_bonusScoreCalculator = new NewBonusScoreCalculator();

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
            UpdateBonusScoreOptions();
            int score = m_scoreCalculator.Evaluate(m_piecePlacement.GetPieces());
            score += m_bonusScoreCalculator.Evaluate(m_piecePlacement.GetPieces());
            m_estimatedScore.Value = score;
        }

        private void UpdateBonusScoreOptions()
        {
            m_bonusScoreCalculator.FloatToInt = m_floatToInt;
            m_bonusScoreCalculator.ScoreMultiply = m_bonusMultiply;
        }

        private void Update()
        {
            if (!m_isPlaying) return;

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
            CancelMovingPiece();
            UpdateEstimatedScore();
            PlayNew();
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