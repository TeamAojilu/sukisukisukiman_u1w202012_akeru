using UnityEngine;

namespace Unity1Week202012
{
    public class SamplePointerInput : IPointerInput
    {
        /// <summary>スペースを押していたらフリックしたことにする</summary>
        public bool Flick(out Vector2 flickVelocity)
        {
            flickVelocity = Vector2.down; // 方向はテキトー.
            return Input.GetKey(KeyCode.Space);
        }
        
        public bool PointerDown() => Input.GetMouseButtonDown(0);
        
        public bool PointerUp() => Input.GetMouseButtonUp(0);
    }
}