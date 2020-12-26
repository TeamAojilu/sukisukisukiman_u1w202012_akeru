using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class AchievementPublish_highScore : MonoBehaviour
    {
        [SerializeField]private GameEventString m_achievementEvent;
        [SerializeField] private GameEventListener m_submitEvent;
        [SerializeField] private ReadonlyInt m_score;
        [SerializeField]private List<int> m_achieveScoreList = new List<int>();

        [SerializeField] private string m_achieveId = "HighScore_{0}";


        private void Start()
        {
            m_submitEvent.Subscribe(EvaluationAchievement).DisposeOnDestroy(gameObject);
        }

        void EvaluationAchievement()
        {
            foreach(var score in m_achieveScoreList)
            {
                if (m_score.Value > score)
                {
                    var id = m_achieveId.Replace("{0}", score.ToString());
                    m_achievementEvent.Publish(id);
                }
            }
        }

    }
}