using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using UnityEngine.UI;

namespace Unity1Week202012.Aojilu.Title
{
    public class DisplayRecordCanvas : MonoBehaviour, IDisplayContent
    {
        [SerializeField] GameObject m_myCanvas;

        [SerializeField] Text m_rankText;
        [SerializeField] Text m_TotalScore;
        [SerializeField] Text m_MaxScore;
        [SerializeField] Text m_TotalSukiman;
        [SerializeField] Text m_MaxSukiman;
        [SerializeField] Text m_PlayNum;
        [SerializeField] Text m_AchievementNum;

        private int achievementNum;
        private void Awake()
        {
            AojiluService_Title.RankingDisplay = this;
        }
        public void Display()
        {
            m_myCanvas.SetActive(true);

            GetRecords();
        }

        private void GetRecords()
        {
            var RankText = Services.UserRankCalculator.GetRank();

            m_rankText.text = RankText;
            m_TotalScore.text = AojiluService.DataSaver.PlaySaveData.TotalScore.ToString();
            m_MaxScore.text = AojiluService.DataSaver.PlaySaveData.MaxScore.ToString();
            m_TotalSukiman.text = AojiluService.DataSaver.PlaySaveData.TotalSukimaCount.ToString();
            m_MaxSukiman.text = AojiluService.DataSaver.PlaySaveData.MaxSukimaCount.ToString();
            m_PlayNum.text = AojiluService.DataSaver.PlaySaveData.PlayCount.ToString();
            var dataDictionary= AojiluService.DataSaver.PlaySaveData.AchivementDatas;
            achievementNum = 0;
            AchievementTexts achievementTexts = new AchievementTexts();
            foreach (var data in achievementTexts.GetAchievemenents())
            {
                if (dataDictionary.ContainsKey(data.m_id))
                {
                    achievementNum++; 
                }
            }
            m_AchievementNum.text = achievementNum.ToString();
        }

        public void Tweet()
        {
            var tweetText = "実績を"+achievementNum+"個解除!!ランクは"+Services.UserRankCalculator.GetRank()+"!!";
            naichilab.UnityRoomTweet.Tweet ("sukisukisukiman", tweetText, "unity1week","すきすきスキマん");
        }
    }
}