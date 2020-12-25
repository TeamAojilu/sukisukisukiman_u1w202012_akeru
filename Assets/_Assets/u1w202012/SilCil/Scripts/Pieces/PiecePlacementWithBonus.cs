using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SilCilSystem.Variables;

namespace Unity1Week202012
{
    [RequireComponent(typeof(CombinationCalculator))]
    [RequireComponent(typeof(BonusCalculator))]
    public class PiecePlacementWithBonus : MonoBehaviour, IPiecePlacement
    {
        [SerializeField] private GameEventListener m_onSubmit = default;
        [SerializeField] private GameEventBoolListener m_onIsPlayingChanged = default;

        private CombinationCalculator m_combinationCalculator = default;
        private BonusCalculator m_bonusCalculator = default;

        private Dictionary<Piece, PieceData> m_pieceData = new Dictionary<Piece, PieceData>();
        private Dictionary<string, int> m_combinations = new Dictionary<string, int>();

        private void Start()
        {
            m_combinationCalculator = GetComponent<CombinationCalculator>();
            m_onSubmit?.Subscribe(CreateInitialPieces).DisposeOnDestroy(gameObject);
            m_onIsPlayingChanged?.Subscribe(x => { if (x) CreateInitialPieces(); }).DisposeOnDestroy(gameObject);
        }

        private void CreateInitialPieces()
        {
            // 盤面を消去.
            foreach (var piece in m_pieceData.Keys)
            {
                if (piece == null) continue;
                if (piece.gameObject == null) continue;
                Destroy(piece.gameObject);
            }

            Services.Board.Clear();
            Services.PieceConnection.Clear();
            m_pieceData.Clear();
            m_combinations.Clear();

            // 新しく生成.
            foreach (var piece in Services.InitialPieceGenerator.CreateInitialPieces())
            {
                PlacePiece(piece, applyBonusSpace: false);
            }
            ApplyBonusSpace();
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

            if (applyBonusSpace) ApplyBonusSpace();
        }

        public void TrashPiece(Piece piece, Vector2 speed)
        {
            RemovePiece(piece);
            if (m_pieceData.ContainsKey(piece)) m_pieceData.Remove(piece);

            // とりあえず削除で.
            Destroy(piece.gameObject);
        }

        private void ApplyBonusSpace()
        {
            // 盤面の評価を行う.
            m_combinations.Clear();
            Services.Combinations.ForEach(x => x.SetupBeforeEvaluate());
            foreach (var piece in m_pieceData.Values)
            {
                CheckCombinations(piece, ref m_combinations);
            }
            
            m_combinationCalculator.UpdateBoardScore(m_combinations);
        }

        private PieceData GetPieceData(Piece piece, Vector2Int[] positions)
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