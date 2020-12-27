using System;
using System.Linq;
using UnityEngine;
using SilCilSystem.Variables;
using Random = UnityEngine.Random;

namespace Unity1Week202012
{
    public class BanParameterSetter : MonoBehaviour
    {
        [Serializable]
        private class RankInfo
        {
            public string m_id = default;
            public string[] m_enableParameters = default;
        }

        [SerializeField] private TextAsset m_paramterText = default;
        [SerializeField] private RankInfo[] m_rankInfos = default;

        [Header("Events")]
        [SerializeField] private GameEventListener m_onEvaluated = default;
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;

        [Header("Variables")]
        [SerializeField] private VariableFloat m_count = default;
        [SerializeField] private VariableBool[] m_colors = default;
        [SerializeField] private VariableBool[] m_shapes = default;
        [SerializeField] private VariableBool[] m_sizes = default;

        private BanParameterReader m_reader = default;

        private void Start()
        {
            m_reader = new BanParameterReader();
            m_reader.ParseParameters(m_paramterText.text);

            m_onEvaluated?.Subscribe(SetParameters)?.DisposeOnDestroy(gameObject);
            m_onIsPlayingChanged?.Subscribe(_ => SetParameters())?.DisposeOnDestroy(gameObject);
        }

        private void SetParameters()
        {
            string rank = Services.UserRankCalculator.GetRank();
            var enables = m_rankInfos.FirstOrDefault(x => x.m_id == rank)?.m_enableParameters;
            if (enables == null || enables.Length == 0) return;

            string key = enables[Random.Range(0, enables.Length)];
            var parameter = m_reader.Parameters[key];

            m_count.Value = parameter.m_count;
            for (int i = 0; i < parameter.Colors.Count; i++) m_colors[i].Value = parameter.Colors[i];
            for (int i = 0; i < parameter.Shapes.Count; i++) m_shapes[i].Value = parameter.Shapes[i];
            for (int i = 0; i < parameter.Sizes.Count; i++) m_sizes[i].Value = parameter.Sizes[i];
        }
    }
}