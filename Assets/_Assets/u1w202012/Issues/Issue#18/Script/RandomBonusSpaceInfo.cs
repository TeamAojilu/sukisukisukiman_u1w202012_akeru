using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using Unity1Week202012.Aojilu;

namespace Unity1Week202012
{
    public class RandomBonusSpaceInfo : MonoBehaviour, IBonusSpaceInfo
    {
        [SerializeField] private GameEventListener m_OnSubmitListener;

        [Header("デバッグ")]
        [SerializeField] private bool m_isOutLog=false;
        private PieceData m_nowPieceData;

        private List<Vector2Int[]> m_PositionDataList;

        private void Start()
        {
            Services.BonusSpaceInfo = this;
            //ピースの配置データはIPieceDataList継承クラスで生成　現状登録されているtextを書き換えると変わるようになっている
            m_PositionDataList = GetComponent<IPieceDataList>().m_PositionDataList;
            m_nowPieceData = CreateRandomPiece();
            m_OnSubmitListener.Subscribe(()=>m_nowPieceData = CreateRandomPiece()).DisposeOnDestroy(gameObject);

        }

        public IEnumerable<PieceData> GetBonusPiece()
        {
            yield return m_nowPieceData;
        }

        PieceData CreateRandomPiece()
        {
            int colorIndex = UnityEngine.Random.Range(0, Constants.ColorNames.Count);
            int shapeIndex = UnityEngine.Random.Range(0, Constants.ShapeNames.Count);
            int positionDataIndex = UnityEngine.Random.Range(0, m_PositionDataList.Count);
            if(m_isOutLog)OutLog(m_PositionDataList[positionDataIndex]);
            return PieceData.Create(Constants.ColorNames[colorIndex], Constants.ShapeNames[shapeIndex], m_PositionDataList[positionDataIndex]);
        }


        void OutLog(Vector2Int[] positions)
        {
            string log = "";
            foreach (var pos in positions)
            {
                log += $"{pos},";
            }
            Debug.Log(log);
        }
    }
}