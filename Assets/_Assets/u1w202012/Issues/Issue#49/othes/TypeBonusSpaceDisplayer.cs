using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using UnityEngine.UI;

namespace Unity1Week202012
{
    public class TypeBonusSpaceDisplayer : MonoBehaviour
    {


        [SerializeField] private List<Transform> m_parentList;
        [SerializeField] private Image m_piecePrefab;
        [SerializeField] private float m_blockSize = 1f;


        [SerializeField] private List<ScriptablePieceBlockInfo> m_blockInfoHolderList = default;
        [Header("イベントオブジェクト")]
        [SerializeField] private GameEventListener m_displayTimeingEvent;
        [SerializeField] private GameEventBoolListener m_gameStartEvent;



        private List<Vector2Int> m_directionList = new List<Vector2Int>() {

            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };

        private void Start()
        {
            m_displayTimeingEvent.Subscribe(Display).DisposeOnDestroy(gameObject);
            m_gameStartEvent.Subscribe((start) => { if (start) Display(); }).DisposeOnDestroy(gameObject);
        }

        //画面表示
        void Display()
        {
            var pieceInfoList = Services.BonusSpaceInfo.GetBonusPiece().ToArray();
            foreach (var parent in m_parentList)
            {
                foreach (Transform tr in parent)
                {
                    Destroy(tr.gameObject);
                }
            }

            for (int i = 0; i < m_parentList.Count; i++)
            {
                if (i < pieceInfoList.Length) m_parentList[i].gameObject.SetActive(true);
                else m_parentList[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < pieceInfoList.Length; i++)
            {

                var targetBlockInfo = m_blockInfoHolderList.FirstOrDefault(b => b.BlockInfo.m_colorName == pieceInfoList[i].m_color && b.BlockInfo.m_shapeName == pieceInfoList[i].m_shape);

                foreach (var pos in pieceInfoList[i].m_positions)
                {
                    var position = new Vector3(pos.x, pos.y, 0f) * m_blockSize;
                    var block = Instantiate(m_piecePrefab, position, Quaternion.identity, m_parentList[i].transform);
                    block.GetComponent<RectTransform>().localPosition = position;
                    if (targetBlockInfo != null) block.sprite=GetAppropriateSprite(pos, pieceInfoList[i].m_positions, targetBlockInfo.BlockInfo);
                }
            }

        }

        Sprite GetAppropriateSprite(Vector2Int checkPos, IEnumerable<Vector2Int> posDataList, ConnectPieceObjectFactory.BlockInfo blockInfo)
        {
            var checkPosList = m_directionList.Select(x => x + checkPos).ToArray();
            int decideNumber = 0;
            //２進数
            //隣にブロックがあるならその桁は1
            for (int i = 0; i < 4; i++)
            {
                if (!posDataList.Contains(checkPosList[i])) decideNumber += 1 << i;
            }

            switch (decideNumber)
            {
                case 0: return blockInfo.sprite_0000;
                case 1: return blockInfo.sprite_0001;
                case 2: return blockInfo.sprite_0010;
                case 3: return blockInfo.sprite_0011;
                case 4: return blockInfo.sprite_0100;
                case 5: return blockInfo.sprite_0101;
                case 6: return blockInfo.sprite_0110;
                case 7: return blockInfo.sprite_0111;
                case 8: return blockInfo.sprite_1000;
                case 9: return blockInfo.sprite_1001;
                case 10: return blockInfo.sprite_1010;
                case 11: return blockInfo.sprite_1011;
                case 12: return blockInfo.sprite_1100;
                case 13: return blockInfo.sprite_1101;
                case 14: return blockInfo.sprite_1110;
                case 15: return blockInfo.sprite_1111;
            }
            return null;
        }
    }
}