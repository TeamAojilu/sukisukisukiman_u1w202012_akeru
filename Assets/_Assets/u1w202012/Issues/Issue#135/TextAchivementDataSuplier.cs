using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu.Title
{
    public class TextAchivementDataSuplier : MonoBehaviour, IAchivementTextSupplier
    {
        private void Start()
        {
            AojiluService_Title.AchivementTextSupplier = this;
        }

        public Dictionary<string, bool> GetAchivementDataList()
        {
            var dataDictionary= AojiluService.DataSaver.PlaySaveData.AchivementDatas;
            var result = new Dictionary<string, bool>();

            AchievementTexts achievementTexts = new AchievementTexts();
            foreach (var data in achievementTexts.GetAchievemenents())
            {
                
                if (dataDictionary.ContainsKey(data.m_id))
                {
                    result.Add(data.m_displayName, dataDictionary[data.m_id]); 
                }
                else
                {
                    result.Add(data.m_displayName, false); 
                }
            }

            return result;
        }
        
        
    }
}