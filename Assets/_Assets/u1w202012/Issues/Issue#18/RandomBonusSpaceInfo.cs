using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class RandomBonusSpaceInfo : MonoBehaviour, IBonusSpaceInfo
    {
        private void Start()
        {
            Services.BonusSpaceInfo = this;
        }

        public IEnumerable<PieceData> GetBonusPiece()
        {
            throw new NotImplementedException();
        }
    }
}