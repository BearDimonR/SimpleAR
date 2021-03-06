namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public class Pawn : Figure
    {
        public override bool[,] PossibleMoves()
        {
            var moves = new bool[BoardManager.Size.x, BoardManager.Size.y];

            var x = Cell.x;
            var y = Cell.y;

            var forward = colour == FigureColour.White ? x - 1 : x + 1;
            var forward2 = colour == FigureColour.White ? x - 2 : x + 2;

            var up = y + 1;
            var down = y - 1;

            var edgeX = BoardManager.Size.x;
            var edgeY = BoardManager.Size.y;

            if (!(forward >= edgeX || forward < 0))
            {
                Figure figure;
                if (!(up >= edgeY || up < 0))
                {
                    figure = boardManager.FigurePositions[forward, up];
                    if (figure != null && figure.colour != colour)
                        moves[forward, up] = true;
                }

                if (!(down >= edgeY || down < 0))
                {
                    figure = boardManager.FigurePositions[forward, down];
                    if (figure != null && figure.colour != colour)
                        moves[forward, down] = true;
                }

                figure = boardManager.FigurePositions[forward, y];
                if (figure != null) return moves;
                moves[forward, y] = true;
            }

            if (!(forward2 >= edgeX || forward2 < 0)
                && boardManager.FigurePositions[forward2, y] == null
                && StartCell.Equals(Cell))
                moves[forward2, y] = true;
            return moves;
        }
    }
}