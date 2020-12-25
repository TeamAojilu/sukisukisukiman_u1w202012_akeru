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
        [SerializeField]private GameDataNCMB m_ncmb;
        [SerializeField]private int score;

        [ContextMenu("save")]
        void Save()
        {
            var data = new NCMBPlayerData();
            data.m_maxScore = score;
            m_ncmb.Save(data);
        }
    }
}