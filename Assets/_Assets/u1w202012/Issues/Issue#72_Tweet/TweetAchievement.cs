using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class TweetAchievement : MonoBehaviour
    {
        [SerializeField] private GameEventString m_onAchieved = default;
        [SerializeField] private GameEventListener m_onTweeted = default;

        private void Start()
        {
            m_onTweeted?.Subscribe(() => m_onAchieved?.Publish("press_tweet")).DisposeOnDestroy(gameObject);
        }
    }
}