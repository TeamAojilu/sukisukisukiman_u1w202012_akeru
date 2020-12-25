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
        [SerializeField]private string playerName;
        [SerializeField] private int maxScore;
        [SerializeField] private int totalScore;
        [SerializeField] private int playCount;
        [SerializeField] private int maxSukimaCount;
        [SerializeField] private int totalSukimaCount;


        #region ISaveDataの実装


        public string PlayerName { get => playerName; set => playerName = value; }
        public int MaxScore { get => maxScore; set => maxScore = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }
        public int PlayCount { get => playCount; set => playCount = value; }
        public int MaxSukimaCount { get => maxSukimaCount; set => maxSukimaCount = value; }
        public int TotalSukimaCount { get => totalSukimaCount; set => totalSukimaCount = value; }

        public Dictionary<string, bool> AchivementDatas { get; set; } = new Dictionary<string, bool>();
        #endregion
    }
}