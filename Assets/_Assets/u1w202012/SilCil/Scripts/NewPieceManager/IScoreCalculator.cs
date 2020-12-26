using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IScoreCalculator
    {
        int Evaluate(IEnumerable<PieceData> pieces);
    }
}