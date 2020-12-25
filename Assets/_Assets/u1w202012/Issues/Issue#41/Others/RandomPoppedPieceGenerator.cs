using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class RandomPoppedPieceGenerator : MonoBehaviour, IPoppedPieceGenerator
    {

        [SerializeField] private List<ReadonlyBool> m_colorList;
        [SerializeField] private List<ReadonlyBool> m_shapeList;
        [SerializeField] private List<ReadonlyBool> m_sizeList;
        private PieceData m_nowPiece;

        private List<Vector2Int[][]> m_positions = new List<Vector2Int[][]>()
        {
            {
                new Vector2Int[][]{ new Vector2Int[] { Vector2Int.zero }}
            },
            {
                new Vector2Int[][]{ new Vector2Int[] { Vector2Int.zero, Vector2Int.right }}
            },
            {
                new Vector2Int[][]
                {
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.right*2 },
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.down },
                }
            },
            {
                new Vector2Int[][]
                {
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 },
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.down },
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, new Vector2Int(1, -1) },
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, new Vector2Int(2, -1) },
                    new Vector2Int[]{ Vector2Int.zero, Vector2Int.right, Vector2Int.down, new Vector2Int(1, -1) },
                }
            },
        };
        private void Start()
        {
            Services.PoppedPieceGenerator = this;
        }

        /// <summary>
        /// 色と形はこちらで決定する
        /// 配置は　「大きさは指定のものだけをしてい」「候補の配置をランダムに回転したものを指定」
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PieceData> GetCandidates()
        {
            var enableColorList = m_colorList.Where(x => x.Value == true).ToArray();
            var enablShapeList = m_shapeList.Where(x => x.Value == true).ToArray();

            //色と形の決定
            int enableColorIndex = UnityEngine.Random.Range(0, enableColorList.Length);
            var colorIndex = m_colorList.IndexOf(enableColorList[enableColorIndex]);
            int enableShapeIndex = UnityEngine.Random.Range(0, enablShapeList.Length);
            var shapeIndex = m_shapeList.IndexOf(enablShapeList[enableShapeIndex]);

            //候補の決定
            for(int i = 0; i < m_sizeList.Count;i++)
            {
                if (!m_sizeList[i]) continue;
                foreach (var data in m_positions[i])
                {
                    var posShape= RotateRandom(data);
                    yield return PieceData.Create(Constants.ColorNames[colorIndex], Constants.ShapeNames[shapeIndex],posShape);
                }
            }
        }


        //Issue34InitialPieceGeneratorのものと同じもの
        private Vector2Int[] RotateRandom(IEnumerable<Vector2Int> vectors)
        {
            int count = UnityEngine.Random.Range(0, 4);
            return vectors.Select(vector =>
            {
                for (int i = 0; i < count; i++)
                {
                    vector = new Vector2Int(vector.y, -vector.x);
                }
                return vector;
            }).ToArray();
        }
    }
}