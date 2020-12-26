using System;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class CombinationCalculator : MonoBehaviour
    {
        private const string Path = "CombinationList";
        private const string Comment = "#";
        private static readonly char[] m_separators = new char[] { ',' };

        [SerializeField] private VariableInt m_estimatedBoardScore = default;
        
        private Dictionary<string, CombinationData> m_combinations = new Dictionary<string, CombinationData>();

        private void Start()
        {
            TextAsset[] tables = Resources.LoadAll<TextAsset>(Path);
            foreach(var table in tables)
            {
                ReadAndAppendCombinationData(table);
            }
        }

        public void UpdateBoardScore(IReadOnlyDictionary<string, int> achievements)
        {
            int scoreSum = 0;
            Services.CombinationsViewer?.Clear();
            foreach(var combo in achievements)
            {
                if (!m_combinations.ContainsKey(combo.Key)) continue;
                var data = m_combinations[combo.Key];
                Services.CombinationsViewer?.Add(data, combo.Value);
                scoreSum += data.m_score * combo.Value;
            }
            m_estimatedBoardScore.Value = scoreSum;
        }

        private void ReadAndAppendCombinationData(TextAsset text)
        {
            foreach(var line in text.text.Split(new char[] { '\r', '\n' }))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith(Comment)) continue;

                var words = line.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 3) continue;
                if (!int.TryParse(words[2], out int score)) continue;

                m_combinations[words[0]] = new CombinationData(score, words[1]);
            }
        }
    }
}