using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu
{
    public class TextAchivementDataSupplier : MonoBehaviour, IAchivementDataSupplier
    {
        [SerializeField]private TextAsset m_dataText;
        public Dictionary<string, bool> AchivementList => ReadText();

        Dictionary<string, bool> ReadText()
        {
            var datas=m_dataText.text.Split('\n');

            return datas.ToDictionary(d => d,d=> false);
        }
    }
}