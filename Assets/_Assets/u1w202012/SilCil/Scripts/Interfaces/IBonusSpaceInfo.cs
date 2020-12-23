using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IBonusSpaceInfo
    {
        IEnumerable<PieceData> GetBonusPiece();
    }
}