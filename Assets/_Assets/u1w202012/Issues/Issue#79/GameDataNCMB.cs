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
        string m_NCMBobject_PlayerName = "name";
        string m_NCMBobject_JsonName = "json";

        public void Save(NCMBPlayerData data,string pname)
        {
            NCMBObject testClass = new NCMBObject(m_NCMBobject_ClassName);
            testClass[m_NCMBobject_PlayerName] = pname;
            testClass[m_NCMBobject_JsonName] = JsonUtility.ToJson(data);
            
            testClass.SaveAsync();
        }

        public NCMBPlayerData Load()
        {
        }
    }
}