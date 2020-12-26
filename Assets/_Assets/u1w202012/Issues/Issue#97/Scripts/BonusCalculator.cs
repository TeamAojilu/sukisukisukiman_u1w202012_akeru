using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Unity1Week202012.Issue97
{
    public class BonusCalculator : IBonusCalculator
    {
        private readonly IScoreCalculator m_bonusCalculator;
        private readonly Func<double, double, double> m_scoreBonusFunc;

        public BonusCalculator(IScoreCalculator bonusCalculator, Func<double, double, double> scoreBonusFunc)
        {
            m_bonusCalculator = bonusCalculator;
            m_scoreBonusFunc = scoreBonusFunc;
        }

        public double Evaluate(double scoreBeforeBonus, IEnumerable<PieceData> pieces)
        {
            var bonus = m_bonusCalculator.Evaluate(pieces);
            return m_scoreBonusFunc.Invoke(scoreBeforeBonus, bonus);
        }
    }

    public class MaxSquareCalculator : IScoreCalculator
    {
        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            if (Services.SpaceChecker == null) return 0;

            int max = 0;
            Vector2Int pos = Vector2Int.zero;
            Vector2Int size = Vector2Int.zero;
            foreach (var space in Services.SpaceChecker.GetIsolatedSpaces())
            {
                var array = space.ToArray();
                if (array.Length <= max) continue;

                foreach(var position in array)
                {
                    (int x, int y) = GetMaxSquareSize(position, array);
                    if (x * y <= max) continue;
                    max = x * y;
                    pos = position;
                    size = new Vector2Int(x, y);
                }
            }

            return max;
        }

        private (int, int) GetMaxSquareSize(Vector2Int start, Vector2Int[] space)
        {
            List<int> xlengths = new List<int>();

            Vector2Int offset = Vector2Int.zero;
            while (space.Contains(start + offset))
            {
                xlengths.Add(GetLengthX(start + offset, space));
                offset += Vector2Int.down;
            }

            int max = int.MinValue;
            int max_x = 0;
            int max_y = 0;
            int dx = int.MaxValue;
            for(int i = 0; i < xlengths.Count; i++)
            {
                dx = Mathf.Min(dx, xlengths[i]);
                int dy = i + 1;
                if(dx + dy > max)
                {
                    max = dx + dy;
                    max_x = dx;
                    max_y = dy;
                }
            }

            return (max_x, max_y);
        }

        private int GetLengthX(Vector2Int start, Vector2Int[] space)
        {
            int dx = 0;
            while (space.Contains(start + Vector2Int.right * dx))
            {
                dx++;
            }
            return dx;
        }        
    }
}