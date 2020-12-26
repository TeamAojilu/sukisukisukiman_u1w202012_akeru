using UnityEngine;

namespace Unity1Week202012
{
    public interface IPointerInput
    {
        bool PointerDown();
        bool PointerUp();
        /// <summary>フリックならtrueを返す. PointerUpがtrueの時のみ呼び出す</summary>
        bool Flick(out Vector2 flickVelocity);
        Vector3 PointerWorldPosition { get; }
    }
}