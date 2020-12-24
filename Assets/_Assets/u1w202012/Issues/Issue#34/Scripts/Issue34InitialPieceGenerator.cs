using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class Issue34InitialPieceGenerator : MonoBehaviour, IInitialPieceGenerator
    {
        [SerializeField] private ReadonlyPropertyFloat m_count = default;
        [SerializeField] private ReadonlyPropertyBool[] m_colors = default;
        [SerializeField] private ReadonlyPropertyBool[] m_shapes = default;
        [SerializeField] private ReadonlyPropertyBool[] m_sizes = default;

        [SerializeField] private Vector2Int m_rangeX = new Vector2Int(-4, 4);
        [SerializeField] private Vector2Int m_rangeY = new Vector2Int(-4, 4);

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
            Services.InitialPieceGenerator = this;
        }

        public IEnumerable<Piece> CreateInitialPieces()
        {
            if (m_colors.All(x => !x)) yield break;
            if (m_shapes.All(x => !x)) yield break;
            if (m_sizes.All(x => !x)) yield break;

            List<PieceData> pieces = new List<PieceData>();
            for(int i = 0; i < m_count; i++)
            {
                var color = Constants.ColorNames[GetRandomIndex(m_colors)];
                var shape = Constants.ShapeNames[GetRandomIndex(m_shapes)];
                var positionGroups = m_positions[GetRandomIndex(m_sizes)];
                var positions = RotateRandom(positionGroups[Random.Range(0, positionGroups.Length)]);
                pieces.Add(PieceData.Create(color, shape, positions));
            }

            foreach(var data in pieces)
            {
                if (!GetOrigin(data, out Vector2Int origin)) continue;
                var piece = Services.PieceObjectFactory.Create(data);
                Services.PiecePosition.SetPiecePosition(piece, origin);
                yield return piece;
            }
        }

        private int GetRandomIndex(ReadonlyPropertyBool[] bools)
        {
            return GetIndices(bools).OrderBy(x => Random.value).First();
        }

        private IEnumerable<int> GetIndices(ReadonlyPropertyBool[] bools)
        {
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i]) yield return i;
            }
        }

        private Vector2Int[] RotateRandom(IEnumerable<Vector2Int> vectors)
        {
            int count = Random.Range(0, 4);
            return vectors.Select(vector =>
            {
                for (int i = 0; i < count; i++)
                {
                    vector = new Vector2Int(vector.y, -vector.x);
                }
                return vector;
            }).ToArray();
        }

        private bool GetOrigin(PieceData pieceData, out Vector2Int origin)
        {
            List<Vector2Int> origins = new List<Vector2Int>();

            for(int x = m_rangeX.x; x < m_rangeX.y; x++)
            {
                for(int y = m_rangeY.x; y < m_rangeY.y; y++)
                {
                    var o = new Vector2Int(x, y);
                    if (!Services.Board.CanPlace(pieceData.m_positions.Select(p => p + o))) continue;
                    origins.Add(o);
                }
            }

            if(origins.Count > 0)
            {
                origin = origins[Random.Range(0, origins.Count)];
                return true;
            }
            else
            {
                origin = Vector2Int.zero;
                return false;
            } 
        }
    }
}