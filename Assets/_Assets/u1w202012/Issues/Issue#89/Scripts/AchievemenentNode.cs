using System;
using System.Collections;
using UnityEngine;
using SilCilSystem.Components.Views;

namespace Unity1Week202012
{
    public class AchievemenentNode : MonoBehaviour
    {
        [SerializeField] private GameObject m_displayText = default;
        [SerializeField] private float m_time = 1f;

        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;

        private IDisplayText DisplayText { get => m_display = m_display ?? m_displayText.GetTextComponent(); }
        private IDisplayText m_display = default;

        public IEnumerator ShowCoroutine(AchievementData data, Action callback)
        {
            DisplayText?.SetText(data.m_displayName);
            yield return new WaitForSeconds(m_time);

            callback?.Invoke();
        }
    }
}