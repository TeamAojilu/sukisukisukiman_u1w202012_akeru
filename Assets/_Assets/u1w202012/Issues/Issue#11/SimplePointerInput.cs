using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SimplePointerInput : MonoBehaviour, IPointerInput
    {
        Vector2 touchStartPos;
        Vector2 touchEndPos;

        [SerializeField] private float m_flickSpeed=5.0f;
        [SerializeField] private float m_flickThreshold = 30;//フリック判定の閾値

        bool m_isPointerDownNow = false;


        private void Start()
        {
            Services.PointerInput = this;
        }

        public bool Flick(out Vector2 flickVelocity)
        {
            var moveVector = touchEndPos - touchStartPos;
            flickVelocity =moveVector.normalized*m_flickSpeed;

            return moveVector.sqrMagnitude>m_flickThreshold*m_flickThreshold;
        }
        public bool PointerDown() => Input.GetMouseButtonDown(0);

        public bool PointerUp() => Input.GetMouseButtonUp(0);

        private void Update()
        {
            if (m_isPointerDownNow)
            {
                if (PointerUp())
                {
                    m_isPointerDownNow = false;
                    touchEndPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
            }
            else
            {
                if (PointerDown())
                {
                    m_isPointerDownNow = true;
                    touchStartPos= new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
            }
        }
    }
}