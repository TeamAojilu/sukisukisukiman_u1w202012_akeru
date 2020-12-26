using System.Collections.Generic;
namespace Unity1Week202012.Aojilu
{
    public interface IAchivementDataSupplier
    {
        Dictionary<string,bool> AchivementList { get; }
    }
}