using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    [CreateAssetMenu(fileName = "ScriptablePieceInfo", menuName = "ScriptableObjects/ScriptablePieceInfo")]
    public class ScriptablePieceBlockInfo : ScriptableObject, IHavePieceBlockInfo
    {
        public ConnectPieceObjectFactory.BlockInfo BlockInfo => m_blockInfo;
        [SerializeField] ConnectPieceObjectFactory.BlockInfo m_blockInfo;
    }
}