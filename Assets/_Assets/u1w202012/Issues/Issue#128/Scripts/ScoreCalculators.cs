using System.Linq;
using System.Collections.Generic;
using System;

namespace Unity1Week202012.Issue128
{
    public class SummentionScore : IScoreCalculator
    {
        private readonly IScoreCalculator[] m_scoreCalculators;

        public SummentionScore(IEnumerable<IScoreCalculator> scoreCalculators)
        {
            m_scoreCalculators = scoreCalculators.ToArray();
        }

        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            return m_scoreCalculators.Sum(x => x.Evaluate(pieces));
        }
    }

    public class ScoreMultiplier : IScoreCalculator
    {
        private readonly double m_multiply = default;
        private IScoreCalculator m_scoreCalculator = default;

        public ScoreMultiplier(double multiply, IScoreCalculator scoreCalculator)
        {
            m_multiply = multiply;
            m_scoreCalculator = scoreCalculator;
        }

        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            return m_multiply * m_scoreCalculator.Evaluate(pieces);
        }
    }

    public class SameColorAverageCalculator : IScoreCalculator
    {
        private readonly string m_color;
        public event EventHandler<double> OnEvaluated;
        public event EventHandler<IEnumerable<int>> OnGroupChecked;

        public SameColorAverageCalculator(string color)
        {
            m_color = color;
        }

        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            HashSet<PieceData> hasChecked = new HashSet<PieceData>();

            List<int> counts = new List<int>();
            foreach(var piece in pieces)
            {
                if (piece == null) continue;
                if (piece.m_color != m_color) continue;
                if (hasChecked.Contains(piece)) continue;

                HashSet<PieceData> group = CombinationUtility.GetGroup(piece, x => x.m_color == m_color);
                counts.Add(group.Sum(x => x.m_positions.Length));
                hasChecked.UnionWith(group);
            }

            OnGroupChecked?.Invoke(this, counts);

            var value = (counts.Count == 0) ? 0 : counts.Average();
            OnEvaluated?.Invoke(this, value);
            return value;
        }
    }

    public class SameShapeAverageCalculator : IScoreCalculator
    {
        private readonly string m_shape;
        public event EventHandler<double> OnEvaluated;
        public event EventHandler<IEnumerable<int>> OnGroupChecked;

        public SameShapeAverageCalculator(string shape)
        {
            m_shape = shape;
        }

        public double Evaluate(IEnumerable<PieceData> pieces)
        {
            HashSet<PieceData> hasChecked = new HashSet<PieceData>();

            List<int> counts = new List<int>();
            foreach (var piece in pieces)
            {
                if (piece == null) continue;
                if (piece.m_shape != m_shape) continue;
                if (hasChecked.Contains(piece)) continue;

                HashSet<PieceData> group = CombinationUtility.GetGroup(piece, x => x.m_shape == m_shape);
                counts.Add(group.Sum(x => x.m_positions.Length));
                hasChecked.UnionWith(group);
            }

            OnGroupChecked?.Invoke(this, counts);
            var value = (counts.Count == 0) ? 0 : counts.Average();
            OnEvaluated?.Invoke(this, value);
            return value;
        }
    }
}