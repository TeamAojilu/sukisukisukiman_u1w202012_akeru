namespace Unity1Week202012
{
    public interface IAchievementTexts
    {
        bool TryGetAchievementData(string id, out AchievementData data);
    }
}