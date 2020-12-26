using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Math;

namespace Unity1Week202012
{
    public class AchivementNodeAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 m_start_slideIn = new Vector3(0f, 0f, 1f);
        [SerializeField] private Vector3 m_end_slideIn = new Vector3(1f, 1f, 1f);
        [SerializeField] private Vector3 m_start_moveLine = new Vector3(0f, 0f, 1f);
        [SerializeField] private Vector3 m_end_moveLine = new Vector3(1f, 1f, 1f);
        [SerializeField] private InterpolationCurve m_curve = default;
        [SerializeField] private float m_animationSpeed = 1f;

        RectTransform rectTr;

        public bool NowAnimation { get; private set; } = false;


        public IEnumerator SlideIn()
        {
            rectTr = GetComponent<RectTransform>();
            while (NowAnimation)
            {
                yield return null;
            }
            NowAnimation = true;
            rectTr.localPosition = m_start_slideIn;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * m_animationSpeed;
                rectTr.localPosition = Vector3.LerpUnclamped(m_start_slideIn, m_end_slideIn, m_curve.Evaluate(t));
                yield return null;
            }

            rectTr.localPosition = m_end_slideIn;

            NowAnimation = false;
        }

        public IEnumerator MoveUp()
        {
            rectTr = GetComponent<RectTransform>();
            while (NowAnimation)
            {
                yield return null;
            }
            NowAnimation = true;
            rectTr.localPosition = m_start_moveLine;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * m_animationSpeed;
                rectTr.localPosition = Vector3.LerpUnclamped(m_start_slideIn, m_end_slideIn, m_curve.Evaluate(t));
                yield return null;
            }

            rectTr.localPosition = m_end_moveLine;

            NowAnimation = false;
        }
    }
}