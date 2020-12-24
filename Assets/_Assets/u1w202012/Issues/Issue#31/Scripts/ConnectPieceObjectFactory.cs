using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public interface IHavePieceBlockInfo
    {
        ConnectPieceObjectFactory.BlockInfo BlockInfo { get; }
    }

    public class ConnectPieceObjectFactory : MonoBehaviour, IPieceObjectFactory
    {
        [Serializable]
        public class BlockInfo
        {
            public string m_colorName = Constants.ColorNames[0];
            public string m_shapeName = Constants.ShapeNames[0];
            public Sprite sprite_0000;
            public Sprite sprite_0001;
            public Sprite sprite_0010;
            public Sprite sprite_0011;
            public Sprite sprite_0100;
            public Sprite sprite_0101;
            public Sprite sprite_0110;
            public Sprite sprite_0111;
            public Sprite sprite_1000;
            public Sprite sprite_1001;
            public Sprite sprite_1010;
            public Sprite sprite_1011;
            public Sprite sprite_1100;
            public Sprite sprite_1101;
            public Sprite sprite_1110;
            public Sprite sprite_1111;
        }

        [SerializeField] private Piece m_parentPrefab = default;
        [SerializeField] private SpriteRenderer m_blockPrefab = default;
        [SerializeField] private List<ScriptablePieceBlockInfo> m_blockInfoHolderList = default;
        [SerializeField] private float m_blockSize = 1f;

        private List<Vector2Int> m_directionList = new List<Vector2Int>() {

            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };

        private void Awake()
        {
            Services.PieceObjectFactory = this;
        }

        public Piece Create(PieceData pieceData)
        {
            if (pieceData == null) return null;

            var targetBlockInfo = m_blockInfoHolderList.FirstOrDefault(b => b.BlockInfo.m_colorName == pieceData.m_color && b.BlockInfo.m_shapeName == pieceData.m_shape).BlockInfo;
            if (targetBlockInfo == null) return null;

            var parent = Instantiate(m_parentPrefab, Vector3.zero, Quaternion.identity);
            parent.m_pieceData = pieceData;

            foreach (var pos in pieceData.m_positions)
            {
                var position = new Vector3(pos.x, pos.y, 0f) * m_blockSize;
                var blockSr = Instantiate(m_blockPrefab, position, Quaternion.identity, parent.transform);
                blockSr.sprite = GetAppropriateSprite(pos,pieceData.m_positions,targetBlockInfo);
            }

            return parent;
        }

        Sprite GetAppropriateSprite(Vector2Int checkPos,IEnumerable<Vector2Int> posDataList, BlockInfo blockInfo)
        {
            var checkPosList = m_directionList.Select(x => x + checkPos).ToArray();
            int decideNumber = 0;
            //２進数
            //隣にブロックがあるならその桁は1
            for(int i = 0; i < 4; i++)
            {
                if (!posDataList.Contains(checkPosList[i])) decideNumber += 1 << i;
            }

            switch (decideNumber)
            {
                case 0 : return blockInfo.sprite_0000;
                case 1 : return blockInfo.sprite_0001;
                case 2 : return blockInfo.sprite_0010;
                case 3 : return blockInfo.sprite_0011;
                case 4 : return blockInfo.sprite_0100;
                case 5 : return blockInfo.sprite_0101;
                case 6 : return blockInfo.sprite_0110;
                case 7 : return blockInfo.sprite_0111;
                case 8 : return blockInfo.sprite_1000;
                case 9 : return blockInfo.sprite_1001;
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