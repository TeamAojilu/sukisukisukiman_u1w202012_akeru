using UnityEngine;

namespace Unity1Week202012
{
    public class SampleAchievementNotification : IAchievementNotification
    {
        public void Show(AchievementData data)
        {
            Debug.Log($"【実績解除】{data.m_id}: {data.m_displayName}");
        }
    }
}