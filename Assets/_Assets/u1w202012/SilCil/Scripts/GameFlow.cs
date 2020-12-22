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
        [SerializeField] private VariableBool m_isHoldingPiece = default;
        [SerializeField] private VariableInt m_score = default;
        [SerializeField] private VariableFloat m_time = default;

        [Header("Initialize")]
        [SerializeField] private float m_timeLimit = 30f;

        [Header("Next Scenes")]
        [SerializeField] private string m_nextScene = "Main";

        private IEnumerator Start()
        {
            InitializeVariables();
            yield return SceneLoader.WaitLoading;

            m_isPlaying.Value = true;
            yield return new WaitWhile(() => m_isPlaying);

            SceneLoader.LoadScene(m_nextScene);
        }

        private void InitializeVariables()
        {
            m_isPlaying.Value = false;
            m_isHoldingPiece.Value = false;
            m_score.Value = 0;
            m_time.Value = m_timeLimit;
        }
    }
}