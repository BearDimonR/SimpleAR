using System.Collections.Generic;
using SimpleAR.Examples.Chess.Scripts.Figures;
using SimpleAR.Examples.Chess.Scripts.Utils;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    [RequireComponent(typeof(VisualizationManager))]
    public class BoardManager : MonoBehaviour
    {
        public List<GameObject> activeFigures = new List<GameObject>(Size.x * 5);

        public bool isWhiteTurn = true;
        
        private bool[,] _allowedMoves = new bool[Size.x, Size.y];

        private VisualizationManager _visualizationManager;

        public static IntCell Size { get; } = new IntCell(8, 8);

        [field: SerializeReference] public IntCell SelectedCell { get; set; } = new IntCell(-1, -1);

        public Figure[,] FigurePositions { get; set; } = new Figure[Size.x, Size.y];

        public Figure SelectedFigure { get; set; }
        public VisualizationManager VisualizationManager => _visualizationManager;

        protected void Awake()
        {
            _visualizationManager = gameObject.GetComponent<VisualizationManager>();
            _visualizationManager.boardManager = this;
        }

        public void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            InstantiateAllFigures();
        }

        private void InstantiateAllFigures()
        {
            InstantiateSide(1, 0, FigureColour.Black);
            InstantiateSide(Size.y - 2, Size.y - 1, FigureColour.White);
        }

        private void InstantiateSide(int rowPawn, int rowKing, FigureColour colour)
        {
            for (var i = 0; i < Size.y; ++i) InstantiateFigure(rowPawn, i, FigureType.Pawn, colour);
            InstantiateFigure(rowKing, 0, FigureType.Rook, colour);
            InstantiateFigure(rowKing, 7, FigureType.Rook, colour);

            InstantiateFigure(rowKing, 1, FigureType.Knight, colour);
            InstantiateFigure(rowKing, 6, FigureType.Knight, colour);

            InstantiateFigure(rowKing, 2, FigureType.Bishop, colour);
            InstantiateFigure(rowKing, 5, FigureType.Bishop, colour);

            InstantiateFigure(rowKing, 3, FigureType.Queen, colour);
            InstantiateFigure(rowKing, 4, FigureType.King, colour);
        }

        private void InstantiateFigure(int rowX, int cellY, FigureType type, FigureColour colour)
        {
            var figureObject = _visualizationManager.InstantiateFigure(rowX, cellY, type, colour.Bool());
            var figure = figureObject.GetComponent<Figure>();
            figure.colour = colour;
            figure.Cell = new IntCell(rowX, cellY);
            figure.StartCell = new IntCell(rowX, cellY);
            figure.boardManager = this;

            // add to arrays
            FigurePositions[figure.Cell.x, figure.Cell.y] = figure;
            activeFigures.Add(figureObject);
        }

        public void SelectFigure(int x, int y)
        {
            if (SelectedFigure != null && x == SelectedFigure.Cell.x && y == SelectedFigure.Cell.y)
            {
                Deselect();
                return;
            }

            var pos = FigurePositions[x, y];
            if (pos == null || pos.colour.Bool() != isWhiteTurn) return;

            var hasAtLeastOneMove = false;
            _allowedMoves = FigurePositions[x, y].PossibleMoves();

            for (var i = 0; i < Size.x; i++)
            for (var j = 0; j < Size.y; j++)
                if (_allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;

                    i = 7;
                    break;
                }

            if (!hasAtLeastOneMove) return;

            SelectedFigure = pos;
            _visualizationManager.HighlightCells(_allowedMoves);
        }

        private void Deselect()
        {
            _visualizationManager.ClearHighlightCells();
            SelectedFigure = null;
        }

        public bool MoveFigure(int x, int y)
        {
            if (SelectedFigure == null)
                return false;

            var isOk = true;
            
            if (x != -1 && y != -1 && _allowedMoves[x, y])
            {
                var c = FigurePositions[x, y];
                if (c != null && c.colour.Bool() != isWhiteTurn)
                {
                    activeFigures.Remove(c.gameObject);
                    Destroy(c.gameObject);

                    if (c.figureType == FigureType.King)
                    {
                        EndGame();
                        return true;
                    }
                }

                FigurePositions[SelectedFigure.Cell.x, SelectedFigure.Cell.y] = null;
                _visualizationManager.MoveFigure(SelectedFigure, x, y);
                SelectedFigure.Cell = new IntCell(x, y);
                FigurePositions[x, y] = SelectedFigure;
                isWhiteTurn = !isWhiteTurn;
            }
            else
            {
                isOk = false;
            }

            _visualizationManager.ClearHighlightCells();
            SelectedFigure = null;
            return isOk;
        }

        private void EndGame()
        {
            Debug.Log(isWhiteTurn ? "White team won!" : "Black team won!");
            foreach (var go in activeFigures)
                Destroy(go);
            isWhiteTurn = true;

            _visualizationManager.ClearHighlightCells();

            InstantiateAllFigures();
        }
    }
}