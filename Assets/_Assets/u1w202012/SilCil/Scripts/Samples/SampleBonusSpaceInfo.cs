using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SampleBonusSpaceInfo : IBonusSpaceInfo
    {
        /// <summary>常にred, 0の1マス</summary>
        public IEnumerable<PieceData> GetBonusPiece()
        {
            yield return PieceData.Create(Constants.ColorNames[0], Constants.ShapeNames[0], new Vector2Int[] { Vector2Int.zero, });
        }
    }
}