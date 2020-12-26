using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Issue97
{
    public class ScoreCalculatorsSetter : MonoBehaviour
    {
        [SerializeField] private GameEventFloatListener[] m_onFloatChanged = default;

        [SerializeField] private ReadonlyPropertyFloat m_scoreMultiply = new ReadonlyPropertyFloat(100f);
        [SerializeField] private ReadonlyPropertyFloat m_bonusMultiply = new ReadonlyPropertyFloat(50f);
        
        private void Start()
        {
            SetCalculators();
            foreach(var listen in m_onFloatChanged)
            {
                listen?.Subscribe(x => SetCalculators())?.DisposeOnDestroy(gameObject);
            }
        }

        private void SetCalculators()
        {
            Services.ScoreCalculator = new SummentionScore(GetScoreCalculators());
            Services.BonusCalculator = new BonusCalculator(new MaxSquareCalculator(), (s, b) => s + b * m_bonusMultiply);
        }

        private IEnumerable<IScoreCalculator> GetScoreCalculators()
        {
            foreach (var color in Constants.ColorNames)
            {
                yield return new ScoreMultiplier(m_scoreMultiply, new SameColorAverageCalculator(color));
            }
            foreach(var shape in Constants.ShapeNames)
            {
                yield return new ScoreMultiplier(m_scoreMultiply, new SameShapeAverageCalculator(shape));
            }
        }
    }
}