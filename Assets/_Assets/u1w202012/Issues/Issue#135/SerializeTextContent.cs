using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1Week202012.Aojilu.Title
{
    public class SerializeTextContent : MonoBehaviour, ITextContent
    {
        [SerializeField] Text targetText;
        public void DisplayText(string text)
        {
            targetText.text = text;
        }
    }
}