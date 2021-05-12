namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public class Rook: Figure
    {
        public override bool[,] PossibleMoves()
        {
            bool[,] moves = new bool[BoardManager.Size.X, BoardManager.Size.Y];
            CheckDirection(ref moves, 1, 0);
            CheckDirection(ref moves, -1, 0);
            CheckDirection(ref moves, 0, 1);
            CheckDirection(ref moves, 0, -1);
            return moves;
        }
    }
}