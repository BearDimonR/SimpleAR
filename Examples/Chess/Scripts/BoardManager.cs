using System.Collections.Generic;
using SimpleAR.Examples.Chess.Scripts.Figures;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    [RequireComponent(typeof(VisualizationManager))]
    public class BoardManager : MonoBehaviour
    {

        public static IntCell Size { get; } = new IntCell(8, 8);
        
        [SerializeReference]
        private IntCell _selectedCell = new IntCell(-1, -1);

        public IntCell SelectedCell
        {
            get => _selectedCell;
            set => _selectedCell = value;
        }
        

        private bool[,] _allowedMoves = new bool[Size.X, Size.Y];
        public Figure[,] FigurePositions { get; set; } = new Figure[Size.X, Size.Y];
        
        public Figure SelectedFigure { get; set; }

        public List<GameObject> activeFigures = new List<GameObject>(Size.X * 5);

        protected VisualizationManager _visualizationManager;
        public VisualizationManager VisualizationManager => _visualizationManager;

        public bool isWhiteTurn = true;

        protected void Awake()
        {
            _visualizationManager = gameObject.GetComponent<VisualizationManager>();
            _visualizationManager.boardManager = this;
        }

        public void Start()
        {
            StartGame();
        }
        public void StartGame()
        {
            InstantiateAllFigures();
        }

        public void InstantiateAllFigures()
        {
            InstantiateSide(1, 0, FigureColour.Black);
            InstantiateSide(Size.Y - 2, Size.Y - 1, FigureColour.White);
        }

        private void InstantiateSide(int rowPawn, int rowKing, FigureColour colour)
        {
            for (int i = 0; i < Size.Y; ++i)
            {
                InstantiateFigure(rowPawn, i, FigureType.Pawn, colour);
            }
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
            FigurePositions[figure.Cell.X, figure.Cell.Y] = figure;
            activeFigures.Add(figureObject);
        }

        private void Update()
        {
            // Vector3 handPoint = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;
            //
            //
            // Ray ray = new Ray(handPoint, Vector3.down);
            // //Debug.DrawRay(handPoint, Vector3.down * 100f, Color.red);
            // if (Physics.Raycast(ray, out RaycastHit hit, 20f, 1 << LayerMask.NameToLayer("ChessBoard")))
            // {
            //     _selectedCell = _visualizationManager.PositionToCell(hit.point);
            // }

        }

        public void SelectFigure(int x, int y)
        {
            if (SelectedFigure != null && x == SelectedFigure.Cell.X && y == SelectedFigure.Cell.Y)
            {
                Deselect();
                return;
            }

            var pos = FigurePositions[x, y];
            if (pos == null || pos.colour.Bool() != isWhiteTurn) return;

            bool hasAtLeastOneMove = false;
            _allowedMoves = FigurePositions[x, y].PossibleMoves();

            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    if (_allowedMoves[i, j])
                    {
                        hasAtLeastOneMove = true;

                        i = 7;
                        break;
                    }
                }
            }
            
            if (!hasAtLeastOneMove) return;

            SelectedFigure = pos;
            _visualizationManager.HighlightCells(_allowedMoves);
            
        }

        public void Deselect()
        {
            _visualizationManager.ClearHighlightCells();
            SelectedFigure = null;
        }

        public bool MoveFigure(int x, int y)
        {
            if(SelectedFigure == null)
                return false;
            
            bool isOk = true;

            if(_allowedMoves[x,y])
            {
                Figure c = FigurePositions[x, y];
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

                FigurePositions[SelectedFigure.Cell.X, SelectedFigure.Cell.Y] = null;
                _visualizationManager.MoveFigure(SelectedFigure, x, y);
                SelectedFigure.Cell = new IntCell(x,y);
                FigurePositions[x, y] = SelectedFigure;
                isWhiteTurn = !isWhiteTurn;
            }
            else
                isOk = false;

            _visualizationManager.ClearHighlightCells();
            SelectedFigure = null;
            return isOk;

        }

        public void EndGame()
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