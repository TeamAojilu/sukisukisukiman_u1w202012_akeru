using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class AchievementTexts : IAchievementTexts
    {
        private const string Path = "AchievementList";
        private const string Comment = "#";
        private static readonly char[] m_separators = new char[] { ',', };

        private Dictionary<string, AchievementData> m_achievements = new Dictionary<string, AchievementData>();

        public bool TryGetAchievementData(string id, out AchievementData data)
        {
            return m_achievements.TryGetValue(id, out data);
        }

        public AchievementTexts()
        {
            foreach (var achievementTextAsset in Resources.LoadAll<TextAsset>(Path))
            {
                AddToList(achievementTextAsset.text);
            }
        }

        private void AddToList(string text)
        {
            foreach (var line in text.Split(new char[] { '\r', '\n' }))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith(Comment)) continue;

                string[] words = line.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 2) continue;

                if (m_achievements.ContainsKey(words[0]))
                {
                    Debug.Log($"実績id重複によりスキップ：{words[0]}");
                    continue;
                }
                m_achievements.Add(words[0], new AchievementData(words[0], words[1]));
            }
        }
    }
}