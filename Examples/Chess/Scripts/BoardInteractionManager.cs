using System;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class BoardInteractionManager : ActionBase
    {
        public BoardManager boardManager;
        
        public static readonly IntCell NOT_ON_BOARD = new IntCell(-1, -1);
        
        protected override void OnHandClicked()
        {
            var cell = boardManager.SelectedCell;
            if (cell.X < 0
                || cell.X >= BoardManager.Size.X
                || cell.Y < 0
                || cell.Y >= BoardManager.Size.Y)
                return;
            var figure = boardManager.SelectedFigure;
            if(figure != null && (figure.Cell.X != cell.X || figure.Cell.Y != cell.Y))
                boardManager.MoveFigure(cell.X, cell.Y);
            else
                boardManager.SelectFigure(cell.X, cell.Y);
        }
        private void Update()
        {
            Vector3 handPoint = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;
            var camera = Camera.main;

            Vector3 centerScreen = camera.ViewportToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            
            Ray ray = new Ray(centerScreen, handPoint - centerScreen);
            Debug.DrawRay(centerScreen, handPoint - centerScreen * 100f, Color.red);
            if (!Physics.Raycast(ray, out RaycastHit hit, 20f, 1 << LayerMask.NameToLayer("ChessBoard")))
            {
                boardManager.SelectedCell = NOT_ON_BOARD;
                return;
            }
            
            boardManager.SelectedCell = boardManager.VisualizationManager.PositionToCell(hit.point);
        }
    }
}