using System.Collections;
using Unity1Week202012.Aojilu;

namespace Unity1Week202012
{
    public interface IDataSaver
    {
        IPlaySaveData PlaySaveData { get; }

        IEnumerator Load();
        IEnumerator Save();
    }
}