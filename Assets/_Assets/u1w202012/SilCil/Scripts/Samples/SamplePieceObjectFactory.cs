using System;
using System.Linq;
using UnityEngine;

namespace Unity1Week202012
{
    public class SamplePieceObjectFactory : MonoBehaviour, IPieceObjectFactory
    {
        [Serializable]
        private class BlockInfo
        {
            public string m_colorName = Constants.ColorNames[0];
            public string m_shapeName = Constants.ShapeNames[0];
            public GameObject m_prefab = default;
        }

        [SerializeField] private Piece m_parentPrefab = default;
        [SerializeField] private BlockInfo[] m_blockPrefabs = default;
        [SerializeField] private float m_blockSize = 1f;

        private void Awake()
        {
            Services.PieceObjectFactory = this;
        }

        public Piece Create(PieceData pieceData)
        {
            if (pieceData == null) return null;

            var blockPrefab = m_blockPrefabs.FirstOrDefault(b => b.m_colorName == pieceData.m_color && b.m_shapeName == pieceData.m_shape);
            if (blockPrefab?.m_prefab == null) return null;

            var parent = Instantiate(m_parentPrefab, Vector3.zero, Quaternion.identity);
            parent.m_pieceData = pieceData;

            foreach (var pos in pieceData.m_positions)
            {
                var position = new Vector3(pos.x, pos.y, 0f) * m_blockSize;
                var block = Instantiate(blockPrefab.m_prefab, position, Quaternion.identity, parent.transform);
            }

            return parent;
        }
    }
}