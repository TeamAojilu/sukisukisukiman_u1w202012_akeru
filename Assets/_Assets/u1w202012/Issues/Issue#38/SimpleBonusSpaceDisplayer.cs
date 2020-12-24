using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class SimpleBonusSpaceDisplayer : MonoBehaviour
    {
        [SerializeField]private List<Transform> m_parentList;
        [SerializeField]private Image m_piecePrefab;
        [SerializeField]private float m_blockSize = 1f;

        [SerializeField]private GameEventListener m_displayTimeingEvent;
        [SerializeField]private GameEventBoolListener m_gameStartEvent;


        private void Start()
        {
            m_displayTimeingEvent.Subscribe(Display).DisposeOnDestroy(gameObject);
            m_gameStartEvent.Subscribe((start) => { if (start) Display(); }).DisposeOnDestroy(gameObject);
        }

        [ContextMenu("display")]
        void Display()
        {
            var info = Services.BonusSpaceInfo.GetBonusPiece().ToArray();
            foreach (var parent in m_parentList)
            {
                foreach(Transform tr in parent)
                {
                    Destroy(tr.gameObject);
                }
            }

            for (int i = 0; i < m_parentList.Count; i++)
            {
                if (i < info.Length) m_parentList[i].gameObject.SetActive(true);
                else m_parentList[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < info.Length; i++)
            {
                foreach(var pos in info[i].m_positions)
                {
                    var position = new Vector3(pos.x, pos.y, 0f) * m_blockSize;
                    var block = Instantiate(m_piecePrefab, position, Quaternion.identity, m_parentList[i].transform);
                    block.GetComponent<RectTransform>().localPosition = position;
                }
            }

        }
    }
}