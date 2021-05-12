using SimpleAR.Examples.Chess.Scripts.Utils;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class BoardInteractionManager : ActionBase
    {
        public static readonly IntCell NotONBoard = new IntCell(-1, -1);
        public BoardManager boardManager;

        private Camera _cameraMain;

        private void Start()
        {
            _cameraMain = Camera.main;
        }

        private void Update()
        {
            var handPoint = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;

            System.Diagnostics.Debug.Assert(_cameraMain != null, nameof(_cameraMain) + " != null");
            
            var centerScreen = _cameraMain.ViewportToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

            var ray = new Ray(centerScreen, handPoint - centerScreen);
            Debug.DrawRay(centerScreen, handPoint - centerScreen * 100f, Color.red);
            if (!Physics.Raycast(ray, out var hit, 20f, 1 << LayerMask.NameToLayer("ChessBoard")))
            {
                boardManager.SelectedCell = NotONBoard;
                return;
            }

            boardManager.SelectedCell = boardManager.VisualizationManager.PositionToCell(hit.point);
        }

        protected override void OnHandClicked()
        {
            var cell = boardManager.SelectedCell;
            if (cell.x < 0
                || cell.x >= BoardManager.Size.x
                || cell.y < 0
                || cell.y >= BoardManager.Size.y)
                return;
            var figure = boardManager.SelectedFigure;
            if (figure != null && (figure.Cell.x != cell.x || figure.Cell.y != cell.y))
                boardManager.MoveFigure(cell.x, cell.y);
            else
                boardManager.SelectFigure(cell.x, cell.y);
        }
    }
}