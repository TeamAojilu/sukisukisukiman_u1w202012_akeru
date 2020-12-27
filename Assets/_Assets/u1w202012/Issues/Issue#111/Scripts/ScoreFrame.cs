using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Components.Views;

namespace Unity1Week202012.Issue111
{
    public class ScoreFrame : MonoBehaviour
    {
        [SerializeField] private string format = "+0000";
        [SerializeField] private GameObject m_displayText = default;

        private Camera m_camera = default;
        private RectTransform m_transform = default;
        private IDisplayText m_display = default;


        public void SetScore(int score)
        {
            m_display = m_display ?? m_displayText.GetTextComponent();
            m_display.SetText(score.ToString(format));
        }

        public void SetPosition(Vector3 worldPosition)
        {
            m_camera = m_camera ?? Camera.main;
            m_transform = m_transform ?? GetComponent<RectTransform>();
            m_transform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(m_camera, worldPosition);
        }
    }
}