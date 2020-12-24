using System.Collections;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Singletons;

namespace Unity1Week202012
{
    public class GameFlow : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private VariableBool m_isPlaying = default;
        [SerializeField] private VariableBool m_isRanking = default;
        [SerializeField] private VariableBool m_isHoldingPiece = default;
        [SerializeField] private VariableInt m_score = default;
        [SerializeField] private VariableFloat m_time = default;
        [SerializeField] private GameEventListener m_onRetry = default;

        [Header("Initialize")]
        [SerializeField] private ReadonlyPropertyFloat m_timeLimit = new ReadonlyPropertyFloat(30f);

        [Header("Next Scenes")]
        [SerializeField] private string m_nextScene = "Main";

        private void Awake()
        {
            m_onRetry?.Subscribe(OnRetry).DisposeOnDestroy(gameObject);
        }

        private IEnumerator Start()
        {
            InitializeVariables();
            yield return SceneLoader.WaitLoading;

            yield return Services.StartEffect?.EffectCoroutine();

            m_isPlaying.Value = true;
            yield return new WaitWhile(() => m_isPlaying);

            m_isRanking.Value = true;
            yield return new WaitWhile(() => m_isRanking);

            Services.Reset();
            SceneLoader.LoadScene(m_nextScene);
        }

        private void InitializeVariables()
        {
            m_isPlaying.Value = false;
            m_isRanking.Value = false;
            m_isHoldingPiece.Value = false;
            m_score.Value = 0;
            m_time.Value = m_timeLimit;
        }

        private void OnRetry()
        {
            if (!m_isPlaying) return;
            StopAllCoroutines();
            StartCoroutine(Start());
        }
    }
}