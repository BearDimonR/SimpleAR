namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public class Knight: Figure
    {
        public override bool[,] PossibleMoves()
        {
            bool[,] moves = new bool[BoardManager.Size.X, BoardManager.Size.Y];
            CheckDirection(ref moves, 2, -1, 1);
            CheckDirection(ref moves, 2, 1, 1);
            CheckDirection(ref moves, -2, 1, 1);
            CheckDirection(ref moves, -2, -1, 1);
            CheckDirection(ref moves, -1, 2, 1);
            CheckDirection(ref moves, 1, 2, 1);
            CheckDirection(ref moves, 1, -2, 1);
            CheckDirection(ref moves, -1, -2, 1);
            return moves;
        }
    }
}