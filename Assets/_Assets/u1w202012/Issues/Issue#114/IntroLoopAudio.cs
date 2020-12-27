using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class IntroLoopAudio : MonoBehaviour
    {
        [SerializeField] AudioSource _introAudioSource;
        [SerializeField] AudioSource _loopAudioSource;

        private void Awake()
        {
            Play();
        }
        public void Play()
        {
            /*_introAudioSourceも_loopAudioSourceもAudioSource、AudioClipは設定済み*/

            //イントロ部分の再生開始
            _introAudioSource.PlayScheduled(AudioSettings.dspTime);

            //イントロ終了後にループ部分の再生を開始
            _loopAudioSource.PlayScheduled(AudioSettings.dspTime + ((float)_introAudioSource.clip.samples / (float)_introAudioSource.clip.frequency));
        }
    }
}