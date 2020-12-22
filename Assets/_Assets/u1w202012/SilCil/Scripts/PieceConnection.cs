using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class PieceConnection
    {
        private Dictionary<Piece, List<Piece>> neighbors = new Dictionary<Piece, List<Piece>>();
    }
}