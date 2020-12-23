using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SampleBonusSpaceChecker : IBonusSpaceChecker
    {
        /// <summary>ボーナス判定をしない</summary>
        /// <returns>常にnull</returns>
        public IEnumerable<Vector2Int> GetBonusSpaceOrigins(Vector2Int[] bonusSpaceShape)
        {
            return null;
        }
    }
}