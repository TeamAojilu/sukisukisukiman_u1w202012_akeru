using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using UnityEngine.Events;

namespace Unity1Week202012
{
    public class EnableToPlay : MonoBehaviour
    {
        public UnityEvent m_onEnable = default;

        private void OnEnable()
        {
            m_onEnable.Invoke();
        }
    }
}