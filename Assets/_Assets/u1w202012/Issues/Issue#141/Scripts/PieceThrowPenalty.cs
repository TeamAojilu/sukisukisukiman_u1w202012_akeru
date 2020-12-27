using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class PieceThrowPenalty : MonoBehaviour
    {
        [SerializeField] private VariableFloat m_playTime = default;
        [SerializeField] private ReadonlyPropertyFloat m_decrease = new ReadonlyPropertyFloat(5f);
        [SerializeField] private GameEventListener m_OnPieceThrown = default;

        private void Start()
        {
            m_OnPieceThrown?.Subscribe(() =>
            {
                m_playTime.Value -= m_decrease;
            }).DisposeOnDestroy(gameObject);
        }
    }
}