using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class AnimationAchievementPopup : MonoBehaviour, IAchievementNotification
    {
        [SerializeField] private Transform m_parent = default;
        [SerializeField] private AnimationAcievementNode[] m_nodes = default;


        private Transform m_transform = default;

        private int m_count = 0;
        private Queue<AchievementData> m_achievements = new Queue<AchievementData>();

        [SerializeField] RectTransform target;
        [SerializeField]
        NodeAnimation anim;
        Vector3 m_targetFirstLocalPos;

        private void Start()
        {
            m_targetFirstLocalPos = target.localPosition;

            m_transform = transform;
            AchievementsManager.Instance.AchievementNotification = this;
        }

        private void OnDestroy()
        {
            if (AchievementsManager.Instance.AchievementNotification == this as IAchievementNotification)
            {
                AchievementsManager.Instance.AchievementNotification = null;
            }
        }

        bool m_waiting;

        private void Update()
        {
            if (m_achievements.Count == 0) return;

            while (m_achievements.Count != 0 && m_count < m_nodes.Length&&!m_waiting)
            {
                StartCoroutine( AddNode());

                //var data = m_achievements.Dequeue();
                //var node = CreateNode();
                //node.CashedTransform.SetParent(m_parent);
                //node.CashedTransform.SetAsLastSibling();
                //StartCoroutine(node.ShowCoroutine(data, () => Close(node)));
                //m_count++;
            }
        }

        IEnumerator AddNode()
        {
            m_waiting = true;
            if (m_count > 0) yield return VerticalAnim(ResetPos);

            var data = m_achievements.Dequeue();
            var node = CreateNode();
            node.CashedTransform.SetParent(m_parent);
            node.CashedTransform.SetAsFirstSibling();
            //StartCoroutine(node.ShowCoroutine(data, () => Close(node)));
            StartCoroutine(node.ShowCoroutine(data, () => Close(node)));
            m_count++;

            m_waiting = false;
        }

        private void Close(AnimationAcievementNode node)
        {
            StartCoroutine( node.CloseCorutine(()=> {

                node.gameObject.SetActive(false);
                node.CashedTransform.SetParent(m_transform);
                m_count--;
            }));

        }

        public void Show(AchievementData data)
        {
            m_achievements.Enqueue(data);
        }

        private AnimationAcievementNode CreateNode()
        {
            var node = m_nodes.Last(x => !x.gameObject.activeSelf);
            node.gameObject.SetActive(true);
            return node;
        }

        IEnumerator VerticalAnim(Action endAction)
        {
            yield return anim.Animation(); ;
            endAction.Invoke();
        }

        void ResetPos()
        {
            target.localPosition = m_targetFirstLocalPos;
        }
    }
}