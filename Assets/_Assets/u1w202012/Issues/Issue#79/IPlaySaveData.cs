using System.Collections.Generic;

namespace Unity1Week202012.Aojilu
{
    public interface IPlaySaveData
    {
        Dictionary<string, bool> AchivementDatas { get; set; }
        int MaxScore { get; set; }
        int MaxSukimaCount { get; set; }
        int PlayCount { get; set; }
        string PlayerName { get; set; }
        int TotalScore { get; set; }
        int TotalSukimaCount { get; set; }
    }
}