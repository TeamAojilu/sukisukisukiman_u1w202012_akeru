﻿using System;
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
        [SerializeField] private VariableInt m_combinationCount;//コンボの個数
        [SerializeField] private VariableInt m_combinationTypeCount;//種類の個数

        List<GameObject> m_textObjList = new List<GameObject>();

        HashSet<string> m_combinationTypeHash = new HashSet<string>();
        private void Start()
        {
            Services.CombinationsViewer = this;
        }

        public void Add(CombinationData combination, int count)
        {
            var obj = Instantiate(m_textPrefab, m_parent);
            m_textObjList.Add(obj.gameObject);
            obj.text = $"{combination.m_displayText}: {combination.m_score} x {count}";

            try
            {
                var code = combination.m_displayText.Split(' ');
                m_combinationCount.Value += count *int.Parse(code[1]);
                m_combinationTypeHash.Add(code[0]);
                m_combinationTypeCount.Value = m_combinationTypeHash.Count;
            }
            catch (Exception e)
            {
                Debug.LogWarning("コンボ数のテキストが想定と違うため、計算が失敗しました text="+ $"{combination.m_displayText}: {combination.m_score} x {count}");
            }

        }

        public void Clear()
        {
            m_textObjList.ForEach(x => Destroy(x));
            m_textObjList = new List<GameObject>();

            m_combinationCount.Value = 0;
            m_combinationTypeCount.Value = 0;
            m_combinationTypeHash = new HashSet<string>();
        }
    }
}