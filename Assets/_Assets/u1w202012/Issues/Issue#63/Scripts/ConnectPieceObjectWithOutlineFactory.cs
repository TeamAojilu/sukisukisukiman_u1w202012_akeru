using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class ConnectPieceObjectWithOutlineFactory : MonoBehaviour, IPieceObjectFactory
    {
        [Serializable]
        public class BlockInfo
        {
            public string m_colorName = Constants.ColorNames[0];
            public string m_shapeName = Constants.ShapeNames[0];
            public Sprite m_sprite;
        }

        [SerializeField] private Piece m_parentPrefab = default;
        [SerializeField] private SpriteRenderer m_blockPrefab = default;
        [SerializeField] private List<BlockInfo> m_blockInfoHolderList = default;
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

            var targetBlockInfo = m_blockInfoHolderList.FirstOrDefault(b => b.m_colorName == pieceData.m_color && b.m_shapeName == pieceData.m_shape);
            if (targetBlockInfo == null) return null;

            var parent = Instantiate(m_parentPrefab, Vector3.zero, Quaternion.identity);
            parent.m_pieceData = pieceData;

            foreach (var pos in pieceData.m_positions)
            {
                var position = new Vector3(pos.x, pos.y, 0f) * m_blockSize;
                var blockSr = Instantiate(m_blockPrefab, position, Quaternion.identity, parent.transform);
                blockSr.sprite = targetBlockInfo.m_sprite;
            }

            return parent;
        }
    }
}