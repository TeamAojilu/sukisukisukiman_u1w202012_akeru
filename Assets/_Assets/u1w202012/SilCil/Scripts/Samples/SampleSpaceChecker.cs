using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SampleSpaceChecker : ISpaceChecker
    {
        public IEnumerable<IEnumerable<Vector2Int>> GetIsolatedSpaces()
        {
            yield break;
        }
    }
}