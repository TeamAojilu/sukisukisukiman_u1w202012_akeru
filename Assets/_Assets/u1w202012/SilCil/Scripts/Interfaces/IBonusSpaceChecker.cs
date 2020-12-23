using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public interface IBonusSpaceChecker
    {
        IEnumerable<Vector2Int> GetBonusSpaceOrigins(Vector2Int[] bonusSpaceShape);
    }
}