using UnityEngine;

namespace Unity1Week202012
{
    public class SampleCombinationsViewer : ICombinationsViewer
    {
        public void Clear() { }
        
        public void Add(CombinationData combination, int count)
        {
            Debug.Log($"{combination.m_displayText}: {combination.m_score} x {count}");
        }
    }
}