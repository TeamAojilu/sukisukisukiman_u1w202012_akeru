using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu
{
    public class Test_ncmb : MonoBehaviour
    {
        [SerializeField]private NCMBDataSaver m_ncmb;
        [SerializeField]private MyGameSaveData data=new MyGameSaveData();

        [ContextMenu("save")]
        void Save()
        {
            StartCoroutine( m_ncmb.Save(data));
        }

        [ContextMenu("load")]
        void Load()
        {
            StartCoroutine(LoadColutine());
        }

        IEnumerator LoadColutine()
        {
            yield return m_ncmb.Load();
            data = m_ncmb.GetData();
        }
    }
}