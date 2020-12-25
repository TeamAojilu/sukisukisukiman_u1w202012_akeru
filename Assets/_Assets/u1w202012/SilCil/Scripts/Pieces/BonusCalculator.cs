using UnityEngine;
using SilCilSystem.Variables;
using System.Linq;
using System.Collections;
using SilCilSystem.Math;

namespace Unity1Week202012
{
    public class BonusCalculator : MonoBehaviour
    {
        [SerializeField] private VariableInt m_estimatedBoardScore = default;
        [SerializeField] private ReadonlyPropertyFloat m_scoreMultiply = new ReadonlyPropertyFloat(100f);
        [SerializeField] private FloatToInt.CastType m_floatToInt = default;

        public IEnumerator AddBonusScoreCoroutine()
        {
            if (Services.SpaceChecker == null) yield break;
            
            int min = int.MaxValue;
            Vector2Int[] positions = null;
            foreach (var space in Services.SpaceChecker.GetIsolatedSpaces())
            {
                var array = space.ToArray();
                if (array.Length >= min) continue;
                min = array.Length;
                positions = array;
            }

            yield return Services.BonusEffect?.BonusEffectCoroutine();
            m_estimatedBoardScore.Value += m_floatToInt.Cast(min * m_scoreMultiply);
        }
    }
}