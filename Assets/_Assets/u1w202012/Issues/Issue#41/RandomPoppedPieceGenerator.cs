using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class RandomPoppedPieceGenerator : MonoBehaviour, IPoppedPieceGenerator
    {

        private PieceData m_nowPiece;

        private void Start()
        {
            Services.PoppedPieceGenerator = this;
        }

        public IEnumerable<PieceData> GetCandidates()
        {
            yield return PieceData.Create(Constants.ColorNames[0], Constants.ShapeNames[0], new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
            });
            yield return PieceData.Create(Constants.ColorNames[1], Constants.ShapeNames[1], new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
            });
        }
    }
}