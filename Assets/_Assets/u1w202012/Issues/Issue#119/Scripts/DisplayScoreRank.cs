using System.Linq;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Math;
using SilCilSystem.Components.Views;

namespace Unity1Week202012
{
    public class DisplayScoreRank : MonoBehaviour
    {
        [SerializeField] private string m_format = "00";
        [SerializeField] private GameObject[] m_texts = default;
        [SerializeField] private ReadonlyFloat[] m_variables = default;
        [SerializeField] private FloatToInt.CastType m_floatToInt = default;

        private IDisplayText[] m_displayTexts = default;

        private void Start()
        {
            m_displayTexts = m_texts.Select(x => x.GetTextComponent()).ToArray();
        }

        private void Update()
        {
            for(int i = 0; i < m_variables.Length; i++)
            {
                m_displayTexts[i].SetText(m_floatToInt.Cast(m_variables[i]).ToString(m_format));
            }
        }
    }
}