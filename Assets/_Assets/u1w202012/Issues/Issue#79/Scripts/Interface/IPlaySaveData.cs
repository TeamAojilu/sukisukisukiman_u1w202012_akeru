using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IPlaySaveData
    {
        string PlayerName { get; set; }
        int PlayCount { get; set; }
        int MaxScore { get; set; }
        int TotalScore { get; set; }
        int MaxSukimaCount { get; set; }
        int TotalSukimaCount { get; set; }
        Dictionary<string, bool> AchivementDatas { get; set; }
    }
}