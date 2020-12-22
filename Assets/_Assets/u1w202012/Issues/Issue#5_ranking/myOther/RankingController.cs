using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class RankingController : MonoBehaviour
    {
        [SerializeField] private ReadonlyInt m_score;

        [SerializeField] private GameEventBoolListener m_IsRankingChenged;
        [SerializeField] private VariableBool m_IsRanking;

        private void Awake()
        {
            m_IsRankingChenged.Subscribe((isRanking) => {
                if (!isRanking) return;
                OpenRanking();
            }).DisposeOnDestroy(gameObject);
            //IsRankingをfalseにする処理は、直接ボタンに登録した
        }
        void OpenRanking()
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(m_score.Value);
        }

    }
}