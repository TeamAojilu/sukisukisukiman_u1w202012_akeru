using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class PieceTrashAchievement : MonoBehaviour
    {
        [SerializeField] private GameEventString m_onAchived = default;
        [SerializeField] private GameEventListener m_onPieceTrashed = default;

        private void Start()
        {
            m_onPieceTrashed?.Subscribe(() => m_onAchived?.Publish("piece_trashed")).DisposeOnDestroy(gameObject);
        }
    }
}