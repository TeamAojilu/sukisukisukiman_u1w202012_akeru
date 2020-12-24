using UnityEngine;
using UnityEngine.UI;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class TimeLimitCanvas : MonoBehaviour
    {
        [SerializeField] private VariableFloat m_timeLimit = default;
        [SerializeField] private InputField m_inputField = default;

        private void Awake()
        {
            SetTimeLimit(m_inputField.text);
        }

        public void SetTimeLimit(string text)
        {
            if (!float.TryParse(text, out float t)) return;
            m_timeLimit.Value = t;
        }
    }
}