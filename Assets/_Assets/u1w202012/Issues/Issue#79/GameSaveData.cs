﻿using System.Collections.Generic;
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

        #region ISaveDataの実装


        public string PlayerName { get; set; }
        public int MaxScore { get; set; }
        public int TotalScore { get; set; }
        public int PlayCount { get; set; }
        public int MaxSukimaCount { get; set; }
        public int TotalSukimaCount { get; set; }

        public Dictionary<string, bool> AchivementDatas { get; set; } = new Dictionary<string, bool>();
        #endregion
    }
}