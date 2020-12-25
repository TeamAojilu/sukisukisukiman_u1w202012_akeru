using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using NCMB;
using NCMB.Extensions;//内地さんの奴に依存している
using Unity1Week202012;
namespace Unity1Week202012.Aojilu
{

    public class NCMBDataSaver : MonoBehaviour, IDataSaver
    {

        #region constans
        private string OBJECTID_KEY = "objectId";
        private string PLAYERPREFASKEY = "pl";
        private string NCMB_CLASSNAME = "PlayerData";
        #endregion
        #region field
        private string m_objectid = null;
        private string ObjectID
        {
            get { return m_objectid ?? (m_objectid = PlayerPrefs.GetString(PLAYERPREFASKEY, null)); }
            set
            {
                if (m_objectid == value)
                    return;
                PlayerPrefs.SetString(PLAYERPREFASKEY, m_objectid = value);
            }
        }

        private NCMBObject m_recordObject;
        #endregion

        private void Start()
        {
            AojiluService.DataSaver = this;
        }
        #region IDataSaverの実装

        public IPlaySaveData PlaySaveData { get; set; } = new GameSaveData();

        public IEnumerator Save()
        {
            yield return Save((GameSaveData)PlaySaveData);
        }
        IEnumerator Save(GameSaveData data)
        {
            PlaySaveData = data;
            if (m_recordObject == null)
            {
                m_recordObject = new NCMBObject(NCMB_CLASSNAME);
                m_recordObject.ObjectId = ObjectID;
            }

            m_recordObject = ConvertPlayerData2NCMB(data, m_recordObject);

            NCMBException errorResult = null;
            yield return m_recordObject.YieldableSaveAsync(error => errorResult = error);
            if (errorResult != null)
            {
                //NCMBのコンソールから直接削除した場合に、該当のobjectIdが無いので発生する（らしい）
                m_recordObject.ObjectId = null;
                yield return m_recordObject.YieldableSaveAsync(error => errorResult = error); //新規として送信
            }

            //ObjectIDを保存して次に備える
            ObjectID = m_recordObject.ObjectId;
        }
        public IEnumerator Load()
        {
            //objectIDで検索
            var hiScoreCheck = new YieldableNcmbQuery<NCMBObject>(NCMB_CLASSNAME);
            hiScoreCheck.WhereEqualTo(OBJECTID_KEY, ObjectID);
            yield return hiScoreCheck.FindAsync();
            if (hiScoreCheck.Count == 0)
            {
                Debug.LogError($"データが見つかりません objectID={ObjectID}");
                yield break;
            }
            var ncmbObject = hiScoreCheck.Result[0];
            PlaySaveData = ConvertNCMB2PlayerData(ncmbObject);
        }
        #endregion
        #region セーブデータとNCMBのデータのやり取り
        #region キー
        private string NCMB_DATA_PLAYERNAME = "name";
        private string NCMB_DATA_MAXSCORE = "maxScore";
        private string NCMB_DATA_TOTALSCORE = "totalScore";
        private string NCMB_DATA_PLAYCOUNT = "playCount";
        private string NCMB_DATA_MAXSUKIMAN = "maxSukiman";
        private string NCMB_DATA_TOTALSUKIMAN = "totalSukiman";
        private string NCMB_DATA_ACHIVEMENT = "achievement";
        #endregion
        IPlaySaveData ConvertNCMB2PlayerData(NCMBObject ncmbObject)
        {
            var result = new GameSaveData();
            result.PlayerName = ncmbObject[NCMB_DATA_PLAYERNAME].ToString();
            result.MaxScore = int.Parse(ncmbObject[NCMB_DATA_MAXSCORE].ToString());
            result.TotalScore = int.Parse(ncmbObject[NCMB_DATA_TOTALSCORE].ToString());
            result.PlayCount = int.Parse(ncmbObject[NCMB_DATA_PLAYCOUNT].ToString());
            result.MaxSukimaCount = int.Parse(ncmbObject[NCMB_DATA_MAXSUKIMAN].ToString());
            result.TotalSukimaCount = int.Parse(ncmbObject[NCMB_DATA_TOTALSUKIMAN].ToString());
            var json = MiniJSON.Json.Deserialize(ncmbObject[NCMB_DATA_ACHIVEMENT].ToString()) as Dictionary<string, object>;
            result.AchivementDatas = json.ToDictionary(j => j.Key, j=>(bool) j.Value);
            return result;
        }
        NCMBObject ConvertPlayerData2NCMB(GameSaveData data, NCMBObject ncmbObject)
        {
            ncmbObject[NCMB_DATA_PLAYERNAME] = data.PlayerName;
            ncmbObject[NCMB_DATA_MAXSCORE] = data.MaxScore;
            ncmbObject[NCMB_DATA_TOTALSCORE] = data.TotalScore;
            ncmbObject[NCMB_DATA_PLAYCOUNT] = data.PlayCount;
            ncmbObject[NCMB_DATA_MAXSUKIMAN] = data.MaxSukimaCount;
            ncmbObject[NCMB_DATA_TOTALSUKIMAN] = data.TotalSukimaCount;

            ncmbObject[NCMB_DATA_ACHIVEMENT] =  MiniJSON.Json.Serialize(data.AchivementDatas);
            return ncmbObject;
        }
        #endregion

    }
}