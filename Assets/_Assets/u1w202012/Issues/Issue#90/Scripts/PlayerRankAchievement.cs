using System;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    [DefaultExecutionOrder(100)]
    public class PlayerRankAchievement : PlayDataAchievementsPublisher
    {
        private const string Format = "player_rank_{0}";

        [Serializable]
        private class RankInfo
        {
            public string key = default;
            public string[] enables = default;
        }

        [SerializeField] private RankInfo[] m_achievements = default;

        protected override bool CheckAchieved(IPlaySaveData playSaveData, out string[] keys)
        {
            string rank = Services.UserRankCalculator.GetRank();
            var info = m_achievements.FirstOrDefault(x => x.key == rank);
            if(info == null)
            {
                keys = null;
                return false;
            }
            else
            {
                keys = info.enables.Select(x => string.Format(Format, x)).ToArray();
                return true;
            }
        }
    }
}