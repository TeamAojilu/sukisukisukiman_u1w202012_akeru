using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using SilCilSystem.Math;

namespace Unity1Week202012
{
    public class PieceAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 m_start = new Vector3(0f, 0f, 1f);
        [SerializeField] private Vector3 m_end = new Vector3(1f, 1f, 1f);
        [SerializeField] private InterpolationCurve m_curve = default;

        [SerializeField] private float m_animationSpeed = 1f;

        [SerializeField] private Vector2 m_beforeNoiseRenge;

        [SerializeField] AudioClip m_poppedSE;
        private IEnumerator Start()
        {

            yield return StartAnim();
        }

        IEnumerator StartAnim()
        {
            transform.localScale = m_start;
            var waittime = UnityEngine.Random.Range(m_beforeNoiseRenge.x, m_beforeNoiseRenge.y);
            yield return new WaitForSeconds(waittime);
            
            //終わりを合わせるための再生速度の計算
            var notWaitAnimTime = 1 / m_animationSpeed;
            var calcedAnimTime = notWaitAnimTime - waittime;
            var calcedAnimSpeedRate = 1 / calcedAnimTime;

            PlaySE();
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime*calcedAnimSpeedRate;
                transform.localScale = Vector3.LerpUnclamped(m_start, m_end, m_curve.Evaluate(t));
                yield return null;
            }

            transform.localScale = m_end;
        }
        void PlaySE()
        {
            if (m_poppedSE == null) return;
            GetComponent<AudioSource>().PlayOneShot(m_poppedSE);
        }
    }
}