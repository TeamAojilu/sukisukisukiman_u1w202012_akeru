﻿using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    public class TotalSukimaCountAchievement : PlayDataAchievementsPublisher
    {
        private const string Format = "total_sukima_{0}";

        [SerializeField] private int[] m_achievements = default;

        protected override bool CheckAchieved(IPlaySaveData playSaveData, out string[] keys)
        {
            keys = m_achievements.Where(x => x <= playSaveData.TotalSukimaCount).Select(x => string.Format(Format, x)).ToArray();
            return keys.Length != 0;
        }
    }
}