using UnityEngine;
using SilCilSystem.Variables;
using Unity1Week202012.Aojilu;

namespace Unity1Week202012
{
    public class PlayDataUpdater : MonoBehaviour
    {
        [SerializeField] private GameEvent m_onEvaluated = default;
        [SerializeField] private GameEvent m_onPlayDataUpdated = default;

        [SerializeField] private VariableInt m_score = default;
        [SerializeField] private VariableInt m_sukimaCount = default;

        private void Start()
        {
            m_onEvaluated?.Subscribe(OnEvaluated)?.DisposeOnDestroy(gameObject);
        }

        private void OnEvaluated()
        {
            int maxScore = AojiluService.DataSaver.PlaySaveData.MaxScore;
            if(m_score > maxScore)
            {
                if(maxScore != 0) AchievementsManager.Instance?.AchievementNotification?.Show(new AchievementData("best_score", "ベストスコア！"));
                maxScore = m_score;
            }
            AojiluService.DataSaver.PlaySaveData.MaxScore = maxScore;
            AojiluService.DataSaver.PlaySaveData.MaxSukimaCount = Mathf.Max(AojiluService.DataSaver.PlaySaveData.MaxSukimaCount, m_sukimaCount);
            AojiluService.DataSaver.PlaySaveData.PlayCount++;
            AojiluService.DataSaver.PlaySaveData.TotalScore += (int)(m_score * Constants.TotalScoreRate);
            AojiluService.DataSaver.PlaySaveData.TotalSukimaCount += m_sukimaCount;
            m_onPlayDataUpdated?.Publish();
        }
    }
}