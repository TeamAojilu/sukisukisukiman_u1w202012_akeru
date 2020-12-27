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
        private static bool isSoundFirst = true;
        private static List<float> _volumes = new List<float>();
        
        private void Awake()
        {
            AojiluService_Title.SettingDisplay = this;

            if (isSoundFirst)
            {
                // static 変数の初期化
                _volumes.Add(0.5f);
                _volumes.Add(0.5f);
                _volumes.Add(0.5f);
                isSoundFirst = false;
            }
            for (int i = 0; i< 3; i++)
            {
                m_sliderList[i].minValue = 0;
                m_sliderList[i].maxValue = 1;
                m_sliderList[i].value = _volumes[i];
            }
            SetSetting();
        }
        public void Display()
        {
            m_myCanvas.SetActive(true);

            //GetSetting();
        }
        float ConvertVolume2dB(float volume) => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);

        //void GetSetting()
        //{
        //    if (m_mixier.GetFloat(m_masterPath, out var master))
        //    {
        //        m_sliderList[0].value = master;
        //    }
        //    if (m_mixier.GetFloat(m_bgmPath, out var bgm))
        //    {
        //        m_sliderList[1].value = bgm;
        //    }
        //    if (m_mixier.GetFloat(m_sePath, out var se))
        //    {
        //        m_sliderList[2].value = se;
        //    }
        //}

        public void SetSetting()
        {
            m_mixier.SetFloat(m_masterPath,ConvertVolume2dB( m_sliderList[0].value));
            m_mixier.SetFloat(m_bgmPath,ConvertVolume2dB( m_sliderList[1].value));
            m_mixier.SetFloat(m_sePath,ConvertVolume2dB( m_sliderList[2].value));
        }

        public void OnDisable()
        {
            for (int i = 0; i< 3; i++)
            { 
                _volumes[i] = m_sliderList[i].value;
            }
        }
    }
}