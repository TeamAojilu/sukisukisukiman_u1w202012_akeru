using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class TrashAreaPointerInput : MonoBehaviour, IPointerInput
    {
        [SerializeField]private BoxCollider2D m_trashArea;
        [SerializeField]private Vector2 m_centerPostion = Vector2.zero;
        [SerializeField]private float m_flickSpeed=5.0f;

        private Camera m_camera = default;

        public Vector3 PointerWorldPosition => m_camera.ScreenToWorldPoint(Input.mousePosition);

        private void Start()
        {
            m_camera = Camera.main;
            Services.PointerInput = this;
        }

        public bool Flick(out Vector2 flickVelocity)
        {
            //flickVelocityは、速度は固定。方向は基準点からの方向で決定している
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            flickVelocity = ((Vector2)mousePos - m_centerPostion).normalized*m_flickSpeed;

            //対象のものにhitしたらflick判定を出す
            var cols= Physics2D.OverlapPointAll(mousePos);
            return cols.Contains(m_trashArea);
        }

        public bool PointerDown() => Input.GetMouseButtonDown(0);

        public bool PointerUp() => Input.GetMouseButtonUp(0);
    }
}