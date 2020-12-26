using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SampleOnAchievedListener : MonoBehaviour
    {
        [SerializeField] private GameEventStringListener m_onAchieved = default;

        private void Start()
        {
            m_onAchieved?.Subscribe(x => Debug.Log($"OnAchieved: {x}"));
        }
    }
}