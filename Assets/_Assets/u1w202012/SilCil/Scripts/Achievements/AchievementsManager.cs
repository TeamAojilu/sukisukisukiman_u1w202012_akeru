using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Singletons;
using Unity1Week202012.Aojilu;
using UnityEngine.SceneManagement;

namespace Unity1Week202012
{
    public class AchievementsManager : SingletonMonoBehaviour<AchievementsManager>
    {
        private const string DefaultName = "No Name";

        [SerializeField] private GameEventStringListener m_onAchieved = default;

        [Header("Debug")]
        [SerializeField] private bool m_saveData = false;

        private IDisposable m_disposable = default;
        private Queue<AchievementData> m_achivements = new Queue<AchievementData>();
        private bool m_isServerBusy = false;

        public IAchievementTexts AchievementTexts
        {
            get => m_texts;
            set => m_texts = value ?? m_texts;
        }
        private IAchievementTexts m_texts = default;

        public IAchievementNotification AchievementNotification { get; set; }

        protected override void OnAwake() { }
        protected override void OnDestroyCallback()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            m_disposable?.Dispose();
        }

        private void OnAchieved(string achievementID)
        {
            if (!AchievementTexts.TryGetAchievementData(achievementID, out AchievementData data)) return;
            if (AojiluService.DataSaver.PlaySaveData.AchivementDatas.TryGetValue(achievementID, out bool achieved) && achieved) return;
            m_achivements.Enqueue(data);
        }

        private void Start()
        {
            StartCoroutine(LoadCoroutine());
            AchievementTexts = AchievementTexts ?? new AchievementTexts();
            AchievementNotification = AchievementNotification ?? new SampleAchievementNotification();
            m_disposable = m_onAchieved?.Subscribe(OnAchieved);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if (m_isServerBusy) return;
            if (m_achivements.Count == 0) return;

            while(m_achivements.Count != 0)
            {
                var achive = m_achivements.Dequeue();
                AojiluService.DataSaver.PlaySaveData.AchivementDatas[achive.m_id] = true;
                AchievementNotification?.Show(achive);
            }

            StartCoroutine(SaveCoroutine());
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            StartCoroutine(SaveCoroutine());
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus) return;
            StartCoroutine(SaveCoroutine());
        }

        private IEnumerator SaveCoroutine(bool wait = true)
        {
            if (m_saveData)
            {
                AojiluService.DataSaver.PlaySaveData.PlayerName = AojiluService.DataSaver.PlaySaveData.PlayerName ?? DefaultName;
                yield return BusyCoroutine(AojiluService.DataSaver.Save(), wait: wait);
            }
            yield break;
        }

        private IEnumerator LoadCoroutine(bool wait = true)
        {
            if (m_saveData)
            {
                yield return BusyCoroutine(AojiluService.DataSaver.Load(), wait: wait);
            }
            yield break;
        }
        private IEnumerator BusyCoroutine(IEnumerator coroutine, bool wait = true)
        {
            if (wait) yield return new WaitWhile(() => m_isServerBusy);

            m_isServerBusy = true;
            yield return coroutine;
            m_isServerBusy = false;
        }
    }
}