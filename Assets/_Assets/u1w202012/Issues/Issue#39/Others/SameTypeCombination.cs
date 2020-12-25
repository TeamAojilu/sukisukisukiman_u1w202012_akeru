using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SameTypeCombination : MonoBehaviour,ICombination
    {
        private const int MinCount = 2;
        private const string KeyFormat = "SameType_{0}_{1}";
        private HashSet<PieceData> m_checked = new HashSet<PieceData>();

        private void Start()
        {
            Services.Combinations.Add(this);
        }

        public void SetupBeforeEvaluate()
        {
            m_checked.Clear();
        }

        public string Evaluate(PieceData pieceData)
        {
            if (pieceData == null) return null;
            if (m_checked.Contains(pieceData)) return null;

            string shape = pieceData.m_shape;
            HashSet<PieceData> group = CombinationUtility.GetGroup(pieceData, x => x.m_shape == shape);
            m_checked.UnionWith(group);

            if (group.Count >= MinCount)
            {
                return string.Format(KeyFormat, shape, group.Count);
            }
            else
            {
                return null;
            }
        }
    }
}