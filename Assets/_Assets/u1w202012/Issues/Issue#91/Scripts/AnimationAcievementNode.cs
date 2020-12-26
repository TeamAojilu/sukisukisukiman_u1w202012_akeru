using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Components.Views;

namespace Unity1Week202012
{
    public class AnimationAcievementNode : MonoBehaviour
    {
        [SerializeField] private GameObject m_displayText = default;
        [SerializeField] private float m_time = 1f;
        [SerializeField] private NodeAnimation openAnime;
        [SerializeField] private NodeAnimation closeAnime;
        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;

        private IDisplayText DisplayText { get => m_display = m_display ?? m_displayText.GetTextComponent(); }
        private IDisplayText m_display = default;

        public IEnumerator ShowCoroutine(AchievementData data, Action callback)
        {
            DisplayText?.SetText(data.m_displayName);
            yield return openAnime.Animation();
            yield return new WaitForSeconds(m_time);

            callback?.Invoke();
        }

        public IEnumerator CloseCorutine(Action callback)
        {
            yield return closeAnime.Animation();
            callback.Invoke();
        }
    }
}