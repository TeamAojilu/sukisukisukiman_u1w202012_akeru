using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class ConnectionUIData : MonoBehaviour
    {
        [Serializable]
        private class ButtonData
        {
            public Button m_button;
            public GameObject m_canvas;
        }

        [SerializeField] List<ButtonData> m_canbasOpenButtonData;
        [SerializeField] List<ButtonData> m_canbasCloseButtonData;

        private void Awake()
        {
            RegisterCanvasOpen();
            RegisterCanvasClose();
        }

        void RegisterCanvasOpen()
        {
            foreach (var data in m_canbasOpenButtonData)
            {
                data.m_button.onClick.AddListener(() => data.m_canvas.SetActive(true));
            }
        }
        void RegisterCanvasClose()
        {
            foreach (var data in m_canbasCloseButtonData)
            {
                data.m_button.onClick.AddListener(() => data.m_canvas.SetActive(false));
            }
        }
    }
}