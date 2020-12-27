using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SilCilSystem.Variables;
using Unity1Week202012.Aojilu.Title;

namespace Unity1Week202012.Aojilu.Title
{
    public class DisplaySettingCanvas : MonoBehaviour, IDisplayContent
    {
        [SerializeField] GameObject m_myCanvas;
        [SerializeField] AudioMixer m_mixier;

        [SerializeField] List<Slider> m_sliderList = new List<Slider>();

        [SerializeField] private string m_masterPath = "Master";
        [SerializeField] private string m_bgmPath = "BGM";
        [SerializeField] private string m_sePath = "SE";
        private void Awake()
        {
            AojiluService_Title.SettingDisplay = this;
        }
        public void Display()
        {
            m_myCanvas.SetActive(true);

            GetSetting();
        }

        void GetSetting()
        {
            if (m_mixier.GetFloat(m_masterPath, out var master))
            {
                m_sliderList[0].value = master;
            }
            if (m_mixier.GetFloat(m_bgmPath, out var bgm))
            {
                m_sliderList[1].value = bgm;
            }
            if (m_mixier.GetFloat(m_sePath, out var se))
            {
                m_sliderList[2].value = se;
            }
        }

        public void SetSetting()
        {
            m_mixier.SetFloat(m_masterPath, m_sliderList[0].value);
            m_mixier.SetFloat(m_bgmPath, m_sliderList[1].value);
            m_mixier.SetFloat(m_sePath, m_sliderList[2].value);
        }
    }
}