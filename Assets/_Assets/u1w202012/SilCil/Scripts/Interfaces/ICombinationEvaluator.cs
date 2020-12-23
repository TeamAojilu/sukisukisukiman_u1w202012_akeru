using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface ICombinationEvaluator
    {
        IReadOnlyDictionary<string, int> CombinationAchievements { get; }
    }
}