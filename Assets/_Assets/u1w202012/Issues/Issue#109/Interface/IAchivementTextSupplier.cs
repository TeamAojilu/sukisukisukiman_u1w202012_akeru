using System.Collections.Generic;

namespace Unity1Week202012.Aojilu.Title
{
    public interface IAchivementTextSupplier
    {
        Dictionary<string,bool> GetAchivementDataList();
    }
}