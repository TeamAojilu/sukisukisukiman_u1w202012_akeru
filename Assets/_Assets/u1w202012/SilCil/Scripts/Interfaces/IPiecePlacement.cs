using UnityEngine;

namespace Unity1Week202012
{
    public interface IPiecePlacement
    {
        void PlacePiece(Piece piece, bool applyBonusSpace = true);
        void PlacePiece(Piece piece, Vector2Int[] positions, bool applyBonusSpace = true);
        void RemovePiece(Piece piece, bool applyBonusSpace = true);
        void TrashPiece(Piece piece, Vector2 speed);
    }
}