using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SeveralSEPlayer : MonoBehaviour
    {
        [SerializeField] private ReadonlyBool m_isPlaying;
        [SerializeField] private GameEventBoolListener m_isHoldingChenged;
        [SerializeField] private GameEventListener m_pieceTrashed;
        [SerializeField] private GameEventListener m_submit;

        [SerializeField] AudioClip m_SE_hold;
        [SerializeField] AudioClip m_SE_put;
        [SerializeField] AudioClip m_SE_discard;
        [SerializeField] AudioClip m_SE_submit;

        AudioSource m_audioListener;

        private void Start()
        {
            m_audioListener = GetComponent<AudioSource>();

            m_isHoldingChenged.Subscribe((hold) =>
            {
                if (!m_isPlaying.Value) return;
                if (hold) m_audioListener.PlayOneShot(m_SE_hold);
                else m_audioListener.PlayOneShot(m_SE_put);
            }).DisposeOnDestroy(gameObject);

            m_pieceTrashed.Subscribe(() => m_audioListener.PlayOneShot(m_SE_discard)).DisposeOnDestroy(gameObject);

            m_submit.Subscribe(() => m_audioListener.PlayOneShot(m_SE_submit)).DisposeOnDestroy(gameObject);
        }
    }
}