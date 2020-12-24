using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class SamplePoppedPieceGenerator : IPoppedPieceGenerator
    {
        public IEnumerable<PieceData> GetCandidates()
        {
            yield return PieceData.Create(Constants.ColorNames[0], Constants.ShapeNames[0], new Vector2Int[] 
            { 
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
            });
        }
    }
}