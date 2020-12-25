using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class HaveEdgeBoard : MonoBehaviour, IBoard, IBonusSpaceChecker
    {
        [SerializeField] private Vector2Int m_centerPosition = Vector2Int.zero;
        [SerializeField] private Vector2Int m_BoardRangeFromCenter_max = new Vector2Int(3, 3);
        [SerializeField]private Vector2Int m_BoardRangeFromCenter_min = new Vector2Int(-4, -4);

        private List<Vector2Int> m_filled = new List<Vector2Int>();

        private List<Vector2Int> m_directionList = new List<Vector2Int>()
        {
            new Vector2Int(1,0),new Vector2Int(-1,0),new Vector2Int(0,1),new Vector2Int(0,-1)
        };

        private void Start()
        {
            Services.Board = this;
            Services.BonusSpaceChecker = this;
        }
        #region IBoardの実装
        public bool CanPlace(IEnumerable<Vector2Int> positions)
        {
            //空の判定
            bool isBlank = positions.All(pos => m_filled.Contains(pos) == false);
            //範囲の判定
            Vector2Int boardRangeMin = m_BoardRangeFromCenter_min + m_centerPosition;
            Vector2Int boardRangeMax = m_BoardRangeFromCenter_max + m_centerPosition;
            bool isInArea = positions.All(pos =>
              boardRangeMin.x <= pos.x && pos.x <= boardRangeMax.x
              && boardRangeMin.y <= pos.y && pos.y <= boardRangeMax.y);

            return isBlank && isInArea;
        }

        public void Place(IEnumerable<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                if (m_filled.Contains(pos)) continue;
                m_filled.Add(pos);
            }
        }

        public void Remove(IEnumerable<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                m_filled.Remove(pos);
            }
        }

        public void Clear()
        {
            m_filled.Clear();
        }
        #endregion
        #region IBonusSpaceCheckerの実装
        public IEnumerable<Vector2Int> GetBonusSpaceOrigins(Vector2Int[] bonusSpaceShape)
        {
            List<Vector2Int> resultList = new List<Vector2Int>();
            Vector2Int boardRangeMin = m_BoardRangeFromCenter_min + m_centerPosition;
            Vector2Int boardRangeMax = m_BoardRangeFromCenter_max + m_centerPosition;

            for (int y = boardRangeMin.y; y <= boardRangeMax.y; y++)
            {
                for (int x = boardRangeMin.x; x <= boardRangeMax.x; x++)
                {
                    var origin = new Vector2Int(x, y);
                    var checkPositions = bonusSpaceShape.Select(pos => pos + origin).ToArray();
                    if(!CanPlace(checkPositions))continue;//置けるか
                    if (!CheckSurrondingFill(checkPositions)) continue;//周囲が埋まっているか
                    resultList.Add(origin);
                }
            }
            return resultList;
        }

        /// <summary>
        /// putPositionsの周囲にブロックを置けないかどうかを返す
        /// </summary>
        /// <param name="putPositions">置かれる位置</param>
        /// <returns></returns>
        bool CheckSurrondingFill(Vector2Int[] putPositions)
        {
            foreach(var pos in putPositions)
            {
                var checklist = m_directionList.Select(x => x + pos);
                foreach(var check in checklist)
                {
                    //置けない、もしくは自分自身が含んでいるなら調査を続行
                    if (!CanPlace(new List<Vector2Int>() { check }) || putPositions.Contains(check))
                    {

                    }
                    else return false;
                }
            }
            //すべてが違反してなかったらtrue
            return true;
        }
        #endregion
    }
}