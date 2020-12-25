using System.Collections;

namespace Unity1Week202012
{
    public interface IDataSaver<T>
    {
        /// <summary>
        /// ロードされていないときはnullを返す
        /// </summary>
        /// <returns></returns>
        T GetData();
        /// <summary>
        /// データのロード
        /// </summary>
        /// <returns></returns>
        IEnumerator Load();

        /// <summary>
        /// データのセーブ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        IEnumerator Save(T data);
    }
}