using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Components.Views;
using SilCilSystem.Math;

namespace Unity1Week202012
{
    public class PenaltyEffect : MonoBehaviour
    {
        [SerializeField] private ReadonlyFloat m_lossTime = default;
        [SerializeField] private Vector3 m_movement = default;
        [SerializeField] private float m_time = 1f;
        [SerializeField] private InterpolationCurve m_curve = default;

        private Transform m_transform = default;
        private Vector3 m_startPositon = default;
        private IDisplayText m_display = default;

        private void OnEnable()
        {
            m_transform = m_transform ?? transform;
            m_startPositon = m_transform.position;
            m_display = m_display ?? gameObject.GetTextComponent();
            m_display.SetText($"-{m_lossTime.Value}");
            StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {
            float timer = 0f;
            while(timer < m_time)
            {
                timer += Time.deltaTime;
                float rate = Mathf.Clamp01(timer / m_time);
                m_transform.position = Vector3.LerpUnclamped(m_startPositon, m_startPositon + m_movement, m_curve.Evaluate(rate));
                yield return null;
            }
            m_transform.position = m_startPositon;
            gameObject.SetActive(false);
        }
    }
}