using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using System.Collections;
using System.Linq;
using SilCilSystem.Math;

namespace Unity1Week202012.Issue128
{
    public interface IColorGroupCountReciever
    {
        void Apply(string color, IEnumerable<int> counts);
    }

    public interface IShapeGroupCountReciever
    {
        void Apply(string shape, IEnumerable<int> counts);
    }

    public class ScoreCalculatorsSetter : MonoBehaviour, IBonusEffect
    {
        [Header("Score")]
        [SerializeField] private GameEventFloatListener[] m_onFloatChanged = default;
        [SerializeField] private ReadonlyPropertyFloat m_scoreMultiply = new ReadonlyPropertyFloat(100f);
        [SerializeField] private ReadonlyPropertyFloat m_bonusMultiply = new ReadonlyPropertyFloat(50f);
        [SerializeField] private VariableInt m_sukimaCount = default;

        [Header("Level")]
        [SerializeField] private VariableFloat[] m_colorRanks = default;
        [SerializeField] private VariableFloat[] m_shapeRanks = default;

        [Header("BonusEffect")]
        [SerializeField] private BonusCharacter m_bonusCharacterPrefab = default;
        [SerializeField] private Vector2 m_delayRange = new Vector2(0f, 0.5f);
        [SerializeField] private AudioSource[] m_audioSources = default;
        [SerializeField] private float m_waitTimeBefore = 0.5f;
        [SerializeField] private float m_waitTimeAfter = 0.5f;

        [SerializeField] private ScoreFrame m_scoreFrame = default;
        [SerializeField] private FloatToInt.CastType m_floatToInt = default;

        private MaxSquareCalculator m_maxSquareCalculator = default;
        private List<BonusCharacter> m_bonusCharacters = new List<BonusCharacter>();

        private Dictionary<string, VariableFloat> m_colors = new Dictionary<string, VariableFloat>();
        private Dictionary<string, VariableFloat> m_shapes = new Dictionary<string, VariableFloat>();

        private IColorGroupCountReciever m_colorCountReciever = default;
        private IShapeGroupCountReciever m_shapeCountReciever = default;

        private void Start()
        {
            m_colorCountReciever = GetComponent<IColorGroupCountReciever>();
            m_shapeCountReciever = GetComponent<IShapeGroupCountReciever>();

            m_scoreFrame.gameObject.SetActive(false);

            for(int i = 0; i < Constants.ColorNames.Count; i++)
            {
                m_colors[Constants.ColorNames[i]] = m_colorRanks[i];
            }
            for (int i = 0; i < Constants.ShapeNames.Count; i++)
            {
                m_shapes[Constants.ShapeNames[i]] = m_shapeRanks[i];
            }

            SetCalculators();
            foreach(var listen in m_onFloatChanged)
            {
                listen?.Subscribe(x => SetCalculators())?.DisposeOnDestroy(gameObject);
            }

            Services.BonusEffect = this;
        }

        private void SetCalculators()
        {
            Services.ScoreCalculator = new SummentionScore(GetScoreCalculators());
            m_maxSquareCalculator = new MaxSquareCalculator();
            var bonus = new BonusCalculator(m_maxSquareCalculator, (s, b) => s + b * m_bonusMultiply);
            m_maxSquareCalculator.OnEvaluated += (_, x) => 
            { 
                m_scoreFrame.SetScore(m_floatToInt.Cast(m_bonusMultiply * (float)x));
                m_sukimaCount.Value = m_floatToInt.Cast((float)x);
            };
            Services.BonusCalculator = bonus;
        }

        private IEnumerable<IScoreCalculator> GetScoreCalculators()
        {
            int count = 0;
            foreach (var color in Constants.ColorNames)
            {
                count++;
                var calculator = new SameColorAverageCalculator(color);
                calculator.OnEvaluated += (_, x) => { m_colors[color].Value = (float)x; };
                calculator.OnGroupChecked += (_, x) => m_colorCountReciever?.Apply(color, x);
                yield return new ScoreMultiplier(m_scoreMultiply * count, calculator);
            }

            count = 0;
            foreach(var shape in Constants.ShapeNames)
            {
                count++;
                var calculator = new SameShapeAverageCalculator(shape);
                calculator.OnEvaluated += (_, x) => { m_shapes[shape].Value = (float)x; };
                calculator.OnGroupChecked += (_, x) => m_shapeCountReciever?.Apply(shape, x);
                yield return new ScoreMultiplier(m_scoreMultiply * count, calculator);
            }
        }

        public IEnumerator BonusEffectCoroutine()
        {
            int dx = m_maxSquareCalculator.MaxSize.x;
            int dy = m_maxSquareCalculator.MaxSize.y;
            int count = 0;
            int finishCount = 0;
            for (int x = 0; x < dx; x++)
            {
                for(int y = 0; y < dy; y++)
                {
                    count++;
                    var delay = Random.Range(m_delayRange.x, m_delayRange.y);
                    var bonus = GetBonusCharacter();
                    StartCoroutine(bonus.PlayEffect(m_maxSquareCalculator.MaxPosition + new Vector2Int(x, -y), delay, () => 
                    {
                        finishCount++;
                    }));
                    
                    if(count < m_audioSources.Length)
                    {
                        m_audioSources[count].PlayDelayed(delay);
                    }
                }
            }

            m_scoreFrame.gameObject.SetActive(true);
            m_scoreFrame.SetPosition(m_maxSquareCalculator.MaxPosition + new Vector2(dx, -dy) / 2f);
            while (count < finishCount) yield return null;

            yield return new WaitForSeconds(m_waitTimeBefore);
            m_scoreFrame.gameObject.SetActive(false);
            m_bonusCharacters.ForEach(x => x.gameObject.SetActive(false));
            yield return new WaitForSeconds(m_waitTimeAfter);
        }

        private BonusCharacter GetBonusCharacter()
        {
            var bonus = m_bonusCharacters.FirstOrDefault(x => !x.gameObject.activeSelf);
            if(bonus == null)
            {
                bonus = Instantiate(m_bonusCharacterPrefab);
                m_bonusCharacters.Add(bonus);
            }
            else
            {
                bonus.gameObject.SetActive(true);
            }
            return bonus;
        }
    }
}