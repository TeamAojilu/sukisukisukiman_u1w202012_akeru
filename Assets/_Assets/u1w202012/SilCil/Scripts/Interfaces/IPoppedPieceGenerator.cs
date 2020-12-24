using System.Collections.Generic;

namespace Unity1Week202012
{
    public interface IPoppedPieceGenerator
    {
        IEnumerable<PieceData> GetCandidates();
    }
}