﻿using UnityEngine;
using System.Linq;
using SilCilSystem.Math;
using System.Collections.Generic;

namespace Unity1Week202012
{
    public class NewBonusScoreCalculator : IScoreCalculator
    {
        public FloatToInt.CastType FloatToInt { get; set; } = default;
        public float ScoreMultiply { get; set; } = 30f;

        public int Evaluate(IEnumerable<PieceData> pieces)
        {
            if (Services.SpaceChecker == null) return 0;

            int min = int.MaxValue;
            Vector2Int[] positions = null;
            foreach (var space in Services.SpaceChecker.GetIsolatedSpaces())
            {
                var array = space.ToArray();
                if (array.Length >= min) continue;
                min = array.Length;
                positions = array;
            }
            
            return FloatToInt.Cast(min * ScoreMultiply);
        }
    }
}