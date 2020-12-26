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
        [SerializeField] private VariableInt m_score = default;
        [SerializeField] private VariableFloat m_time = default;
        [SerializeField] private GameEventListener m_onTitle = default;

        [Header("Initialize")]
        [SerializeField] private ReadonlyPropertyFloat m_timeLimit = new ReadonlyPropertyFloat(30f);

        [Header("Next Scenes")]
        [SerializeField] private string m_titleScene = "Title";

        private bool m_toTitle = false;

        private void Awake()
        {
            m_onTitle?.Subscribe(OnTitle)?.DisposeOnDestroy(gameObject);
        }

        private IEnumerator Start()
        {
            while (!m_toTitle)
            {
                InitializeVariables();
                yield return SceneLoader.WaitLoading;

                yield return Services.StartEffect?.EffectCoroutine();

                m_isPlaying.Value = true;
                yield return new WaitWhile(() => m_isPlaying);
            }
        }

        private void InitializeVariables()
        {
            m_isPlaying.Value = false;
            m_score.Value = 0;
            m_time.Value = m_timeLimit;
        }

        private void OnTitle()
        {
            if (m_toTitle) return;
            m_toTitle = true;
            StartCoroutine(ToTitleCoroutine());
        }

        private IEnumerator ToTitleCoroutine()
        {
            yield return SceneLoader.WaitLoading;
            Services.Reset();
            SceneLoader.LoadScene(m_titleScene);
        }
    }
}