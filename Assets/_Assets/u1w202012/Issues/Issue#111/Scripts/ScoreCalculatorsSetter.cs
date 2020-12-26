using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using System.Collections;
using System.Linq;

namespace Unity1Week202012.Issue111
{
    public class ScoreCalculatorsSetter : MonoBehaviour, IBonusEffect
    {
        [Header("Score")]
        [SerializeField] private GameEventFloatListener[] m_onFloatChanged = default;
        [SerializeField] private ReadonlyPropertyFloat m_scoreMultiply = new ReadonlyPropertyFloat(100f);
        [SerializeField] private ReadonlyPropertyFloat m_bonusMultiply = new ReadonlyPropertyFloat(50f);

        [Header("BonusEffect")]
        [SerializeField] private BonusCharacter m_bonusCharacterPrefab = default;
        [SerializeField] private Vector2 m_delayRange = new Vector2(0f, 0.5f);
        [SerializeField] private AudioSource[] m_audioSources = default;
        [SerializeField] private float m_waitTimeBefore = 0.5f;
        [SerializeField] private float m_waitTimeAfter = 0.5f;

        private MaxSquareCalculator m_maxSquareCalculator = default;
        private List<BonusCharacter> m_bonusCharacters = new List<BonusCharacter>();

        private void Start()
        {
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
            Services.BonusCalculator = new BonusCalculator(m_maxSquareCalculator, (s, b) => s + b * m_bonusMultiply);
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

        public IEnumerator BonusEffectCoroutine()
        {
            int count = 0;
            int finishCount = 0;
            for(int x = 0; x < m_maxSquareCalculator.MaxSize.x; x++)
            {
                for(int y = 0; y < m_maxSquareCalculator.MaxSize.y; y++)
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

            while (count < finishCount) yield return null;

            yield return new WaitForSeconds(m_waitTimeBefore);
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