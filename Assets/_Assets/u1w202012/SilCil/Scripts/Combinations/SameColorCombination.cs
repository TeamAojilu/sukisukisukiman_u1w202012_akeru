using System.Collections.Generic;

namespace Unity1Week202012
{
    public class SameColorCombination : ICombination
    {
        private const int MinCount = 2;
        private const string KeyFormat = "SameColor_{0}_{1}";
        private HashSet<PieceData> m_checked = new HashSet<PieceData>();

        public void SetupBeforeEvaluate()
        {
            m_checked.Clear();
        }

        public string Evaluate(PieceData pieceData)
        {
            if (pieceData == null) return null;
            if (m_checked.Contains(pieceData)) return null;

            string color = pieceData.m_color;
            HashSet<PieceData> group = CombinationUtility.GetGroup(pieceData, ref m_checked, x => x.m_color == color);

            if (group.Count > MinCount)
            {
                return string.Format(KeyFormat, color, group.Count);
            }
            else
            {
                return null;
            }
        }
    }
}