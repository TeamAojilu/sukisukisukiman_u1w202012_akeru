using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Singletons;
using Unity1Week202012.Aojilu;

namespace Unity1Week202012
{
    public class AchievementsManager : SingletonMonoBehaviour<AchievementsManager>
    {
        [SerializeField] private GameEventStringListener m_onAchieved = default;

        [Header("Debug")]
        [SerializeField] private bool m_saveData = false;

        private IDisposable m_disposable = default;
        private Queue<AchievementData> m_achivements = new Queue<AchievementData>();
        private bool m_isServerBusy = default;

        public IAchievementTexts AchievementTexts
        {
            get => m_texts;
            set => m_texts = value ?? m_texts;
        }
        private IAchievementTexts m_texts = default;

        public IAchievementNotification AchievementNotification
        {
            get => m_notification;
            set => m_notification = value ?? m_notification;
        }
        private IAchievementNotification m_notification = default;

        protected override void OnAwake() { }

        protected override void OnDestroyCallback()
        {
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

            if (m_saveData)
            {
                StartCoroutine(SaveCoroutine());
            }
        }

        private IEnumerator SaveCoroutine() => BusyCoroutine(AojiluService.DataSaver.Save());
        private IEnumerator LoadCoroutine() => BusyCoroutine(AojiluService.DataSaver.Load());
        private IEnumerator BusyCoroutine(IEnumerator coroutine)
        {
            m_isServerBusy = true;
            yield return coroutine;
            m_isServerBusy = false;
        }
    }
}