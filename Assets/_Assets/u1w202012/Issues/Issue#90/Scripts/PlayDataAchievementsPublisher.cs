using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public abstract class PlayDataAchievementsPublisher : MonoBehaviour
    {
        [SerializeField] private GameEventString m_onAchieved = default;
        [SerializeField] private GameEventListener m_onPlayDataUpdated = default;

        private void Start()
        {
            m_onPlayDataUpdated?.Subscribe(OnEvaluated)?.DisposeOnDestroy(gameObject);
        }

        private void OnEvaluated()
        {
            if (!CheckAchieved(Aojilu.AojiluService.DataSaver.PlaySaveData, out string[] keys)) return;

            foreach (var key in keys)
            {
                m_onAchieved?.Publish(key);
            }
        }

        protected abstract bool CheckAchieved(IPlaySaveData playSaveData, out string[] key);
    }
}