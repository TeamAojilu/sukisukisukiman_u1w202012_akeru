using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using NCMB;

namespace Unity1Week202012
{
    [Serializable]
    public class NCMBPlayerData
    {
        [SerializeField] public int m_maxScore;
    }

    public class GameDataNCMB : MonoBehaviour
    {
        string m_NCMBobject_ClassName = "PlayerData";
        string m_NCMBobject_JsonName = "json";

        public void Save(NCMBPlayerData data)
        {
            NCMBObject testClass = new NCMBObject(m_NCMBobject_ClassName);
            testClass["json"] = JsonUtility.ToJson(data);
            
            testClass.SaveAsync();
        }

        public IEnumerable Load()
        {
            yield return null;
        }
    }
}