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

        [SerializeField] AchivementNodeAnimation anim;
        public Transform CashedTransform { get => m_transform = m_transform ?? transform; }
        private Transform m_transform = default;

        private IDisplayText DisplayText { get => m_display = m_display ?? m_displayText.GetTextComponent(); }
        private IDisplayText m_display = default;


        private Action m_closeAction;

        public bool m_animationNow { get { return m_animStack.Count > 0; } }
        Queue<string> m_animStack = new Queue<string>();
        public IEnumerator ShowCoroutine(AchievementData data, Action callback)
        {
            m_animStack.Enqueue("a");
            m_closeAction = callback;

            DisplayText?.SetText(data.m_displayName);
            yield return anim.SlideIn();
            m_startTime = Time.time;

            m_animStack.Dequeue();
            //yield return new WaitForSeconds(m_time);

            //callback?.Invoke();
        }

        public IEnumerator MoveUp()
        {
            m_animStack.Enqueue("a");
            yield return anim.MoveUp();
            m_startTime = Time.time;
            m_animStack.Dequeue();
        }

        float m_startTime;
        private void Update()
        {
            if (m_animationNow) return;
            if (m_startTime + m_time > Time.time)
            {
                m_closeAction.Invoke();
            }
        }
    }
}