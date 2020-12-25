using System.Collections;

namespace Unity1Week202012.Aojilu
{
    public interface INCMBDataSaver
    {
        IPlaySaveData PlaySaveData { get; }

        IEnumerator Load();
        IEnumerator Save();
    }
}