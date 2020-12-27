using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SilCilSystem.Variables;
using SilCilSystem.Singletons;

namespace Unity1Week202012.Aojilu.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField] Button StartButton;
        [SerializeField] Button AchivementButton;
        [SerializeField] Button RankingButton;
        [SerializeField] Button SettingButton;

        [SerializeField] string m_sceneName="Main";
        private IEnumerator Start()
        {
            yield return null;
            StartButton.onClick.AddListener(ChengeScene);
            AchivementButton.onClick.AddListener(()=> { if (AojiluService_Title.AchivementDisplay != null) AojiluService_Title.AchivementDisplay.Display(); });
            RankingButton.onClick.AddListener(()=> { if (AojiluService_Title.RankingDisplay != null) AojiluService_Title.RankingDisplay.Display(); });
            SettingButton.onClick.AddListener(()=> { if (AojiluService_Title.SettingDisplay != null) AojiluService_Title.SettingDisplay.Display(); });
        }

         void ChengeScene()
        {
            SceneLoader.LoadScene(m_sceneName);
        }
    }
}