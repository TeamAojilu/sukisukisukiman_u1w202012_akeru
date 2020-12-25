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

        [SerializeField] GameSaveData data;

        [ContextMenu("save")]
        void Save()
        {
            m_ncmb.PlaySaveData = data;
            StartCoroutine( m_ncmb.Save());
        }

        [ContextMenu("load")]
        void Load()
        {
            StartCoroutine(LoadColutine());

        }

        IEnumerator LoadColutine()
        {
            yield return m_ncmb.Load();
            data =(GameSaveData) m_ncmb.PlaySaveData;
        }
    }
}