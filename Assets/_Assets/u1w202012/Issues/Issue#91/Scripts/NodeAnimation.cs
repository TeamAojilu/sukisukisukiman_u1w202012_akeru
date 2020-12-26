using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Math;

namespace Unity1Week202012
{
    public class NodeAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 m_start_slideIn = new Vector3(0f, 0f, 1f);
        [SerializeField] private Vector3 m_end_slideIn = new Vector3(1f, 1f, 1f);
        [SerializeField] private InterpolationCurve m_curve = default;
        [SerializeField] private float m_animationSpeed = 1f;

        [SerializeField]RectTransform targetRectTr;

        public bool NowAnimation { get; private set; } = false;


        public IEnumerator Animation()
        {
            while (NowAnimation)
            {
                yield return null;
            }
            NowAnimation = true;
            targetRectTr.localPosition = m_start_slideIn;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * m_animationSpeed;
                targetRectTr.localPosition = Vector3.LerpUnclamped(m_start_slideIn, m_end_slideIn, m_curve.Evaluate(t));
                yield return null;
            }

            targetRectTr.localPosition = m_end_slideIn;

            yield return null;
            NowAnimation = false;
        }

    }
}