using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012.Aojilu.Title
{
    public class SettinCanvas : MonoBehaviour, IDisplayContent
    {
        [SerializeField] GameObject canvas;

        private void Awake()
        {
            AojiluService_Title.SettingDisplay = this;
        }
        public void Display()
        {
            canvas.SetActive(true);
        }
    }
}