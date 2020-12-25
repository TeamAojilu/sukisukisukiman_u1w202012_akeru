using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using NCMB;
using NCMB.Extensions;//内地さんの奴に依存している

namespace Unity1Week202012
{

    public class NCMBDataSaver : MonoBehaviour, IDataSaver<MyGameSaveData>
    {
        #region constans
        private string OBJECTID_KEY = "objectId";
        private string PLAYERPREFASKEY = "pl";
        private string NCMB_CLASSNAME = "PlayerData";
        #endregion
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

        private INCMB_PlayerDataConverter m_IplDataConverter;
        private MyGameSaveData m_playerData = new MyGameSaveData();
        private NCMBObject m_recordObject;

        private void Awake()
        {
            m_IplDataConverter = GetComponent<INCMB_PlayerDataConverter>();
        }
        public IEnumerator Save(MyGameSaveData data)
        {
            m_playerData = data;
            if (m_recordObject == null)
            {
                m_recordObject = new NCMBObject(NCMB_CLASSNAME);
                m_recordObject.ObjectId = ObjectID;
            }

            m_recordObject = m_IplDataConverter.ConvertPlayerData2NCMB(data, m_recordObject);

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
            m_playerData = m_IplDataConverter.ConvertNCMB2PlayerData(ncmbObject);
        }

        public MyGameSaveData GetData()
        {
            return m_playerData;
        }

    }
}