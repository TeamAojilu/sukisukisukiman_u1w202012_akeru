using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public interface ISpaceChecker
    {
        IEnumerable<IEnumerable<Vector2Int>> GetIsolatedSpaces();
    }
}