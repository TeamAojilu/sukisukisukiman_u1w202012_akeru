using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;
using System;
using Random = UnityEngine.Random;

namespace Unity1Week202012
{
    public class PieceRespawner : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private ReadonlyBool m_isPlaying = default;
        [SerializeField] private GameEvent m_onPiecePoped = default;
        [SerializeField] private GameEventListener m_onSubmit = default;

        [Header("Pop Settings")]
        [SerializeField] private Vector2Int m_rangeX = new Vector2Int(-4, 4);
        [SerializeField] private Vector2Int m_rangeY = new Vector2Int(-4, 4);
        [SerializeField] private Vector2 m_waitTimeRange = new Vector2(0.3f, 1f);

        private class PopSchedule
        {
            public readonly float m_time;
            public readonly Action<Piece> m_callback;
            public PopSchedule(float time, Action<Piece> callback)
            {
                m_time = time;
                m_callback = callback;
            }
        }

        private Queue<PopSchedule> m_schedules = new Queue<PopSchedule>();
        private float m_timer = 0f;

        public Vector2Int[] DisablePlaces { get; set; }

        public void PopScheduled(Action<Piece> callback)
        {
            float time = Random.Range(m_waitTimeRange.x, m_waitTimeRange.y);
            m_schedules.Enqueue(new PopSchedule(time, callback));
        }

        public void Clear()
        {
            m_schedules.Clear();
            m_timer = 0f;
        }

        private void Start()
        {
            m_onSubmit?.Subscribe(Clear).DisposeOnDestroy(gameObject);
        }

        private void Update()
        {
            if (m_schedules.Count == 0) return;
            if (!m_isPlaying)
            {
                Clear();
                return;
            }

            m_timer += Time.deltaTime;
            if(m_timer > m_schedules.Peek().m_time && TryPop(out Piece piece))
            {
                var schedule = m_schedules.Dequeue();
                schedule.m_callback?.Invoke(piece);
                m_onPiecePoped?.Publish();
                m_timer = 0f;
            }
        }

        private bool TryPop(out Piece created)
        {
            List<PieceData> candidates = new List<PieceData>();
            List<List<Vector2Int>> positions = new List<List<Vector2Int>>();
            foreach (var piece in Services.PoppedPieceGenerator.GetCandidates())
            {
                List<Vector2Int> places = GetEnablePlaces(piece);
                if (places.Count != 0)
                {
                    candidates.Add(piece);
                    positions.Add(places);
                }
            }

            created = null;
            if(candidates.Count != 0)
            {
                var index = Random.Range(0, candidates.Count);
                var data = candidates[index];
                var places = positions[index];
                var pos = places[Random.Range(0, places.Count)];

                created = Services.PieceObjectFactory.Create(data);
                Services.PiecePosition.SetPiecePosition(created, pos);
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Vector2Int> GetEnablePlaces(PieceData piece)
        {
            List<Vector2Int> places = new List<Vector2Int>();
            for (int x = m_rangeX.x; x <= m_rangeX.y; x++)
            {
                for (int y = m_rangeY.x; y <= m_rangeY.y; y++)
                {
                    var pos = new Vector2Int(x, y);
                    if (DisablePlaces?.Any(p => p == pos) == true) continue;
                    
                    if (Services.Board.CanPlace(piece.m_positions.Select(p => p + pos)))
                    {
                        places.Add(pos);
                    }
                }
            }
            return places;
        }
    }
}