using SimpleAR.Examples.Chess.Scripts.Utils;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public abstract class Figure : ActionBase
    {
        public FigureColour colour = FigureColour.Black;
        public FigureType figureType = FigureType.Pawn;

        public string handTag = "Player";
        [SerializeField] public BoardManager boardManager;
        private bool _isInside;
        private Transform _parent;
        public IntCell Cell = new IntCell(-1, -1);
        public IntCell StartCell = new IntCell(-1, -1);

        private Material _material;

        private Color _materialColor;

        protected void Start()
        {
            _parent = transform.parent;
            _material = gameObject.GetComponent<Renderer>().material;
            _materialColor = _material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(handTag) || boardManager.SelectedFigure != null) return;
            _material.color = Color.green;
            _isInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(handTag)) return;
            _material.color = _materialColor;
            _isInside = false;
        }

        public abstract bool[,] PossibleMoves();

        protected override void OnHandGrabbed()
        {
            if (!_isInside || boardManager.isWhiteTurn != (colour == FigureColour.White) || boardManager.SelectedFigure != null) return;
            boardManager.SelectFigure(Cell.x, Cell.y);
            transform.parent = SimpleARHandCollider.Instance.transform;
        }

        protected override void OnHandReleased()
        {
            if (boardManager.SelectedFigure != this)
                return;
            
            gameObject.SetActive(false);
            transform.parent = _parent;
            transform.position = boardManager.VisualizationManager.CellToPosition(Cell.x, Cell.y);
            boardManager.MoveFigure(boardManager.SelectedCell.x, boardManager.SelectedCell.y);
            gameObject.SetActive(true);
        }

        protected void CheckDirection(ref bool[,] moves, int difx, int dify, int lim = int.MaxValue)
        {
            var x = Cell.x;
            var y = Cell.y;

            while (lim > 0)
            {
                x += difx;
                y += dify;

                if (x >= BoardManager.Size.x || x < 0 || y >= BoardManager.Size.y || y < 0) return;

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