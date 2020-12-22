using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private GameEventListener m_SubmitEvent;
        [SerializeField] private GameEventBoolListener m_isPlayingChenged;

        [SerializeField] private ReadonlyInt m_estimatedBoardScore;
        [SerializeField] private VariableInt m_currentScore;
        [SerializeField] private VariableInt m_highScore;

        private void Awake()
        {

            m_SubmitEvent.Subscribe(UpdateScore);
            if(m_isPlayingChenged!=null)m_isPlayingChenged.Subscribe((start)=> {
                if (start) InitScore();
                else UpdateHighScore();
            });
        }

        //スコアの更新
        void UpdateScore()
        {
            m_currentScore.Value += m_estimatedBoardScore;
        }
        //ハイスコアの更新
        void UpdateHighScore()
        {
            if (m_currentScore.Value > m_highScore.Value)
            {
                m_highScore.Value = m_currentScore.Value;
            }
        }

        //スコアの更新」
        void InitScore()
        {
            m_currentScore.Value = 0;
        }

    }
}