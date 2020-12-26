using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IBonusCalculator
    {
        double Evaluate(double scoreBeforeBonus, IEnumerable<PieceData> pieces);
    }
}