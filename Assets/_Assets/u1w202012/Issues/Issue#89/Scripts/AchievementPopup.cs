using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week202012
{
    public class AchievementPopup : MonoBehaviour, IAchievementNotification
    {
        [SerializeField] private Transform m_parent = default;
        [SerializeField] private AchievemenentNode m_nodePrefab = default;
        [SerializeField] private int m_maxCount = 3;

        private Transform m_transform = default;

        private int m_count = 0;
        private List<AchievemenentNode> m_nodes = new List<AchievemenentNode>();
        private Queue<AchievementData> m_achievements = new Queue<AchievementData>();

        private void Start()
        {
            m_transform = transform;
            AchievementsManager.Instance.AchievementNotification = this;
        }

        private void OnDestroy()
        {
            if(AchievementsManager.Instance.AchievementNotification == this as IAchievementNotification)
            {
                AchievementsManager.Instance.AchievementNotification = null;
            }
        }

        private void Update()
        {
            if (m_achievements.Count == 0) return;
            
            while(m_achievements.Count != 0 && m_count < m_maxCount)
            {
                var data = m_achievements.Dequeue();
                var node = CreateNode();
                node.CashedTransform.SetParent(m_parent);
                node.CashedTransform.SetAsLastSibling();
                StartCoroutine(node.ShowCoroutine(data, () => Close(node)));
                m_count++;
            }
        }

        private void Close(AchievemenentNode node)
        {
            node.gameObject.SetActive(false);
            node.CashedTransform.SetParent(m_transform);
            m_count--;
        }

        public void Show(AchievementData data)
        {
            m_achievements.Enqueue(data);
        }

        private AchievemenentNode CreateNode()
        {
            var node = m_nodes.FirstOrDefault(x => !x.gameObject.activeSelf);
            if (node == null)
            {
                node = Instantiate(m_nodePrefab);
                m_nodes.Add(node);
            }
            else
            {
                node.gameObject.SetActive(true);
            }
            return node;
        }
    }
}