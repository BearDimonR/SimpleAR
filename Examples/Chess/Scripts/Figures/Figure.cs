using System;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public abstract class Figure : ActionBase
    {
        public FigureColour colour = FigureColour.Black;
        public FigureType figureType = FigureType.Pawn;
        public IntCell Cell = new IntCell(-1,-1);
        public IntCell StartCell = new IntCell(-1, -1);

        public String handTag = "Player";
        [SerializeField] 
        public BoardManager boardManager;
        private bool _isInside;
        private Transform _parent;

        public abstract bool[,] PossibleMoves();

        protected void Start()
        {
            _parent = transform.parent;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(handTag)) return;
            _isInside = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(handTag)) return;
            _isInside = false;
        }
        
        protected override void OnHandGrabbed()
        {
            if (!_isInside) return;
            boardManager.SelectFigure(Cell.X, Cell.Y);
            transform.parent = SimpleARHandCollider.Instance.transform;
        }

        protected override void OnHandReleased()
        {
            if(boardManager.SelectedFigure != this)
                return;
            
            gameObject.SetActive(false);
            transform.parent = _parent;
            transform.position = boardManager.VisualizationManager.CellToPosition(Cell.X, Cell.Y);
            boardManager.MoveFigure(boardManager.SelectedCell.X, boardManager.SelectedCell.Y);
            gameObject.SetActive(true);
        }

        protected void CheckDirection(ref bool[,] moves, int difx, int dify, int lim = Int32.MaxValue)
        {
            var x = Cell.X;
            var y = Cell.Y;

            while (lim > 0)
            {
                x += difx;
                y += dify;
                
                if ((x >= BoardManager.Size.X || x < 0)
                    || (y >= BoardManager.Size.Y || y < 0)) return;
                
                var f = boardManager.FigurePositions[x, y];
                if (f != null)
                {
                    if (f.colour.Bool() != boardManager.isWhiteTurn)
                        moves[x, y] = true;
                    return;
                }
                moves[x, y] = true;
                --lim;
            }
        }
    }
    
    
}