using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using NCMB;
using NCMB.Extensions;//内地さんの奴に依存している

namespace Unity1Week202012
{
    public class NCMBDataConverter : MonoBehaviour, INCMB_PlayerDataConverter
    {
        #region キー
        private string NCMB_DATA_PLAYERNAME = "name";
        private string NCMB_DATA_MAXSCORE = "maxScore";
        #endregion
        public MyGameSaveData ConvertNCMB2PlayerData(NCMBObject ncmbObject)
        {
            var result = new MyGameSaveData();
            result.m_playerName = ncmbObject[NCMB_DATA_PLAYERNAME].ToString();
            result.m_maxScore = int.Parse(ncmbObject[NCMB_DATA_MAXSCORE].ToString());
            return result;
        }

        public NCMBObject ConvertPlayerData2NCMB(MyGameSaveData data, NCMBObject ncmbObject)
        {
            ncmbObject[NCMB_DATA_PLAYERNAME] = data.m_playerName;
            ncmbObject[NCMB_DATA_MAXSCORE] = data.m_maxScore;
            return ncmbObject;
        }
    }
}