using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IScoreCalculator
    {
        double Evaluate(IEnumerable<PieceData> pieces);
    }
}