using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu
{
    public interface IPieceDataList
    {
        List<Vector2Int[]> m_PositionDataList { get; }
    }
}