using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using TMPro;

namespace Unity1Week202012
{
    public class CountGettableCombinationViewer : MonoBehaviour, ICombinationsViewer
    {
        [SerializeField] private TextMeshProUGUI m_textPrefab;
        [SerializeField] private RectTransform m_parent;
        [SerializeField] private VariableInt m_combinationCount;

        List<GameObject> m_textObjList = new List<GameObject>();
        private void Start()
        {
            Services.CombinationsViewer = this;
        }

        public void Add(CombinationData combination, int count)
        {
            var obj = Instantiate(m_textPrefab, m_parent);
            m_textObjList.Add(obj.gameObject);
            obj.text = $"{combination.m_displayText}: {combination.m_score} x {count}";

            m_combinationCount.Value += count;
        }

        public void Clear()
        {
            m_textObjList.ForEach(x => Destroy(x));
            m_textObjList = new List<GameObject>();

            m_combinationCount.Value = 0;
        }
    }
}