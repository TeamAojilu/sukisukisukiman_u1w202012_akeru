using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Unity1Week202012.Aojilu
{

    /// <summary>
    /// ゲームのセーブデータ
    /// </summary>
    [Serializable]
    public class GameSaveData : IPlaySaveData
    {
        //playerの名前
        [SerializeField] string m_playerName;

        //最大スコア
        [SerializeField] int m_maxScore;
        //合計スコア
        [SerializeField] int m_totalScore;

        //評価回数＝プレイ回数
        [SerializeField] int m_playCount;

        //出した最高スキマん数
        [SerializeField] int m_maxSukimanCount;
        //出したスキマんの合計
        [SerializeField] int m_totalsukimanCount;

        [Serializable]
        public class AchivementData
        {
            public string keyName = "";
            public bool flag = false;

            public AchivementData(string keyName, bool flag)
            {
                this.keyName = keyName;
                this.flag = flag;
            }
        }
        //実績開放フラグ
        [SerializeField]public List<AchivementData> m_AchivementData = new List<AchivementData>();

        #region ISaveDataの実装


        public string PlayerName { get { return m_playerName; } set { m_playerName = value; } }
        public int MaxScore { get { return m_maxScore; } set { m_maxScore = value; } }
        public int TotalScore { get { return m_totalScore; } set { m_totalScore = value; } }
        public int PlayCount { get { return m_playCount; } set { m_playCount = value; } }
        public int MaxSukimaCount { get { return m_maxSukimanCount; } set { m_maxSukimanCount = value; } }
        public int TotalSukimaCount { get { return m_totalsukimanCount; } set { m_totalsukimanCount = value; } }

        public Dictionary<string, bool> AchivementDatas
        {
            get
            {
                return m_AchivementData.ToDictionary(data => data.keyName, data => data.flag);
            }
            set
            {
                m_AchivementData = value.Select(x => new AchivementData(x.Key, x.Value)).ToList();
            }
        }
        #endregion
    }
}