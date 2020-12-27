using System;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    public class UserRankCalculater : MonoBehaviour, IUserRankCalculator
    {
        [Serializable]
        private class UserRank
        {
            public string m_id;
            public int m_achivements;
        }

        [SerializeField] private UserRank[] m_ranks = default;

        private void Start()
        {
            m_ranks = m_ranks.OrderBy(x => x.m_achivements).ToArray();
            Services.UserRankCalculator = this;
        }

        public string GetRank()
        {
            int count = Aojilu.AojiluService.DataSaver.PlaySaveData.AchivementDatas.Count(x => x.Value);

            string id = null;
            foreach (var rank in m_ranks) 
            { 
                if(count >= rank.m_achivements)
                {
                    id = rank.m_id;
                }
                else
                {
                    break;
                }
            }

            return id;
        }
    }
}