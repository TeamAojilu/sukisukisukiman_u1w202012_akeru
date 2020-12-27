using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SoundCountResetter : MonoBehaviour
    {
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;
        [SerializeField] private GameEventListener m_onSubmit = default;
        [SerializeField] private VariableInt m_soundCount = default;

        private void Start()
        {
            m_onSubmit?.Subscribe(ResetCount).DisposeOnDestroy(gameObject);
            m_onIsPlayingChanged?.Subscribe(_ => ResetCount());
        }

        private void ResetCount()
        {
            m_soundCount.Value = 0;
        }
    }
}