using Unity1Week202012.Aojilu.Title;
namespace Unity1Week202012.Aojilu.Title
{
    public static class AojiluService_Title
    {
        public static IAchivementTextSupplier AchivementTextSupplier { get; set; } = new SampleAchivementTextSupplier();

        public static IDisplayContent AchivementDisplay { get; set; }
        public static IDisplayContent SettingDisplay { get; set; }
        public static IDisplayContent RankingDisplay { get; set; }
    }
}