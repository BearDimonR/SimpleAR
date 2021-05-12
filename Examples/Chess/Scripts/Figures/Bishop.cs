namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public class Bishop : Figure
    {
        public override bool[,] PossibleMoves()
        {
            var moves = new bool[BoardManager.Size.x, BoardManager.Size.y];
            CheckDirection(ref moves, 1, 1);
            CheckDirection(ref moves, 1, -1);
            CheckDirection(ref moves, -1, -1);
            CheckDirection(ref moves, -1, 1);
            return moves;
        }
    }
}