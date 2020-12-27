﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Issue128
{
    public class ShapeGroupAchievement : MonoBehaviour, IShapeGroupCountReciever
    {
        [SerializeField] private GameEventString m_onAchieved = default;
        [SerializeField] private GameEventListener m_onEvaluated = default;

        [SerializeField] private int[] m_achievements = default;

        private Dictionary<string, int> maxGroups = new Dictionary<string, int>();

        private void Start()
        {
            m_onEvaluated?.Subscribe(OnEvaluated).DisposeOnDestroy(gameObject);
        }

        private void OnEvaluated()
        {
            foreach(var group in maxGroups)
            {
                foreach(var achieve in m_achievements)
                {
                    if(group.Value >= achieve)
                    {
                        m_onAchieved?.Publish($"{group.Key}_chain_{achieve}");
                    }
                }
            }

            // 単一種判定.
            if (maxGroups.Count(x => x.Value != 0) == 1)
            {
                var only = maxGroups.First(x => x.Value != 0);
                m_onAchieved?.Publish($"{only.Key}_all");
            }
        }

        public void Apply(string shape, IEnumerable<int> counts)
        {
            maxGroups[shape] = (counts.Count() == 0) ? 0 : counts.Max();
        }
    }
}