using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class KeyInput_submit : MonoBehaviour
    {
        [SerializeField]private GameEvent m_onSubmit;
        [SerializeField]private GameEvent m_retry;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_onSubmit.Publish();
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                m_retry.Publish();
            }
        }
    }
}