using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    public class PiecePlacement : MonoBehaviour, IEvaluateCombination
    {
        [SerializeField] private GameEventListener m_onSubmit = default;
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;

        private Dictionary<Piece, PieceData> m_pieceData = new Dictionary<Piece, PieceData>();
        private List<PieceData> m_spaces = new List<PieceData>();
        private Dictionary<string, int> m_combinations = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> CombinationAchievements => m_combinations;

        private void Start()
        {
            Services.EvaluateCombination = this;
            m_onSubmit?.Subscribe(CreateInitialPieces).DisposeOnDestroy(gameObject);
            m_onIsPlayingChanged?.Subscribe(x => { if (x) CreateInitialPieces(); }).DisposeOnDestroy(gameObject);
        }

        public void CreateInitialPieces()
        {
            // 盤面を消去.
            foreach(var piece in m_pieceData.Keys)
            {
                RemovePiece(piece);
                Destroy(piece.gameObject);
            }
            m_pieceData.Clear();
            m_spaces.Clear();
            
            // 新しく生成.
            foreach (var piece in Services.InitialPieceGenerator.CreateInitialPieces())
            {
                PlacePiece(piece, applyBonusSpace: false);
            }
            ApplyBonusSpace();
        }

        public PieceData GetPieceData(Piece piece, Vector2Int[] positions)
        {
            if (m_pieceData.ContainsKey(piece))
            {
                var data = m_pieceData[piece];
                data.m_positions = positions;
            }
            else
            {
                var data = PieceData.Create(piece.m_pieceData, positions);
                m_pieceData.Add(piece, data);
            }

            return m_pieceData[piece];
        }

        public void PlacePiece(Piece piece, bool applyBonusSpace = true)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            PlacePiece(piece, piece.m_pieceData.m_positions.Select(x => x + origin).ToArray(), applyBonusSpace);
        }

        public void PlacePiece(Piece piece, Vector2Int[] positions, bool applyBonusSpace = true)
        {
            PieceData pieceData = GetPieceData(piece, positions);
            Services.PieceConnection.Add(pieceData);
            Services.Board.Place(positions);
            if (applyBonusSpace) ApplyBonusSpace();
        }

        public void RemovePiece(Piece piece, bool applyBonusSpace = true)
        {
            var origin = Services.PiecePosition.GetOriginPosition(piece);
            Services.Board.Remove(Services.PiecePosition.GetBlockPositions(piece, origin));

            if (m_pieceData.ContainsKey(piece))
            {
                Services.PieceConnection.Remove(m_pieceData[piece]);
            }

            if(applyBonusSpace) ApplyBonusSpace();
        }

        public void ApplyBonusSpace()
        {
            foreach (var space in m_spaces)
            {
                Services.Board.Remove(space.m_positions);
                Services.PieceConnection.Remove(space);
            }
            m_spaces.Clear();

            foreach (var bonus in Services.BonusSpaceInfo.GetBonusPiece())
            {
                if (bonus == null) continue;

                var origins = Services.BonusSpaceChecker.GetBonusSpaceOrigins(bonus.m_positions);
                if (origins == null) continue;

                foreach (var origin in origins)
                {
                    var space = PieceData.Create(bonus, bonus.m_positions.Select(p => p + origin).ToArray());
                    m_spaces.Add(space);
                    Services.PieceConnection.Add(space);
                }
            }

            Debug.Log($"Bonus: {m_spaces.Count}");
            
            // 盤面の評価を行う.
            m_combinations.Clear();
            Services.Combinations.ForEach(x => x.SetupBeforeEvaluate());
            foreach(var piece in m_pieceData.Values)
            {
                CheckCombinations(piece, ref m_combinations);
            }
            foreach(var space in m_spaces)
            {
                CheckCombinations(space, ref m_combinations);
            }
        }

        private void CheckCombinations(PieceData piece, ref Dictionary<string, int> combinations)
        {
            foreach (var combo in Services.Combinations)
            {
                var result = combo.Evaluate(piece);
                if (result == null) continue;
                combinations[result] = (combinations.ContainsKey(result)) ? combinations[result] + 1 : 1;
            }
        }
    }
}