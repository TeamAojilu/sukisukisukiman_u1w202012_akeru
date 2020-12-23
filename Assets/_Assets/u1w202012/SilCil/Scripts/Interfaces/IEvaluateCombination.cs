using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IEvaluateCombination
    {
        IReadOnlyDictionary<string, int> CombinationAchievements { get; }
    }
}