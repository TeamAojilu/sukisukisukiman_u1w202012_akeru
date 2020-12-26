using UnityEngine;
using System.Collections;
using System;
using SilCilSystem.Math;

namespace Unity1Week202012.Issue111
{
    public class BonusCharacter : MonoBehaviour
    {
        [Header("Scale")]
        [SerializeField] private Vector3 m_start = new Vector3(0f, 0f, 1f);
        [SerializeField] private Vector3 m_end = new Vector3(1f, 1f, 1f);
        [SerializeField] private InterpolationCurve m_curve = default;

        [Header("positionY")]
        [SerializeField] private Vector2 m_moveRangeY = new Vector2(0f, 1f);
        [SerializeField] private InterpolationCurve m_movingCurve = default;

        [SerializeField] private float m_animationSpeed = 1f;
        [SerializeField] private Vector2 m_beforeNoiseRenge;

        private Transform m_transform = default;

        public IEnumerator PlayEffect(Vector2Int position, float delay, Action callback = null)
        {
            m_transform = m_transform ?? transform;

            m_transform.position = new Vector3(position.x, position.y, 0f);
            var startPos = m_transform.position + Vector3.up * m_moveRangeY.x;
            var endPos = m_transform.position + Vector3.up * m_moveRangeY.y;

            m_transform.localScale = m_start;
            yield return new WaitForSeconds(delay);

            //終わりを合わせるための再生速度の計算
            var notWaitAnimTime = 1 / m_animationSpeed;
            var calcedAnimTime = notWaitAnimTime - delay;
            var calcedAnimSpeedRate = 1 / calcedAnimTime;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * calcedAnimSpeedRate;
                m_transform.position = Vector3.LerpUnclamped(startPos, endPos, m_movingCurve.Evaluate(t));
                m_transform.localScale = Vector3.LerpUnclamped(m_start, m_end, m_curve.Evaluate(t));
                yield return null;
            }

            m_transform.localScale = m_end;
            callback?.Invoke();
        }
    }
}