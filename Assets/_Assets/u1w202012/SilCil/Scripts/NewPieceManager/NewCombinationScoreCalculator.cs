using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class NewCombinationScoreCalculator : IScoreCalculator
    {
        private const string Path = "CombinationList";
        private const string Comment = "#";
        private static readonly char[] m_separators = new char[] { ',' };
        private Dictionary<string, CombinationData> m_combinations = new Dictionary<string, CombinationData>();

        public NewCombinationScoreCalculator()
        {
            TextAsset[] tables = Resources.LoadAll<TextAsset>(Path);
            foreach (var table in tables)
            {
                ReadAndAppendCombinationData(table);
            }
        }
        
        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            Dictionary<string, int> achievements = new Dictionary<string, int>();
            foreach(var combo in Services.Combinations)
            {
                combo.SetupBeforeEvaluate();
            }
            foreach(var piece in pieces)
            {
                CheckCombinations(piece, ref achievements);
            }

            int scoreSum = 0;
            Services.CombinationsViewer?.Clear();
            foreach(var combo in achievements)
            {
                if (!m_combinations.ContainsKey(combo.Key)) continue;
                var data = m_combinations[combo.Key];
                Services.CombinationsViewer?.Add(data, combo.Value);
                scoreSum += data.m_score * combo.Value;
            }
            return scoreSum;
        }
        
        private void CheckCombinations(PieceData piece, ref Dictionary<string, int> combinations)
        {
            foreach (var combo in Services.Combinations)
            {
                var result = combo.Evaluate(piece);
                if (result == null) continue;
                combinations[result] = (combinations.ContainsKey(result)) ? combinations[result] + 1 : 1;
            }
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