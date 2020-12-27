using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Unity1Week202012.Aojilu.Title
{
    public class DisplayAchivementList : MonoBehaviour, IDisplayContent
    {
        [SerializeField] RectTransform m_Parent;
        //[SerializeField] Scrollbar m_scroll;
        [SerializeField] GameObject m_textPrefab;

        IAchivementTextSupplier m_achive;

        [SerializeField] GameObject m_myCanvas;
        void Awake()
        {
            AojiluService_Title.AchivementDisplay = this;
        }

        public void Display()
        {
            InitializeContent();
            m_myCanvas.SetActive(true);
            if (m_achive == null) m_achive = AojiluService_Title.AchivementTextSupplier;
            var dataList =m_achive.GetAchivementDataList().Where(x=>x.Value==true);
            foreach (var data in dataList)
            {
                GameObject obj = Instantiate(m_textPrefab,m_Parent);
                m_textPrefab.GetComponent<ITextContent>().DisplayText(data.Key);
                //m_scroll.
            }
        }
        void InitializeContent()
        {
            foreach (Transform tr in m_Parent)
            {
                Destroy(tr.gameObject);
            }
        }


    }
}