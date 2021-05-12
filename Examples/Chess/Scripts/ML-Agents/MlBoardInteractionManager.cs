using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class MlBoardInteractionManager : MonoBehaviour
    {
        public MlBoardManager boardManager;
        
        private void Update()
        {
            Vector3 handPoint = HandDetector.Instance.HandInfos[0].HandPoints.PalmCentre;
                
                
            Ray ray = new Ray(handPoint, Vector3.down);
            //Debug.DrawRay(handPoint, Vector3.down * 100f, Color.red);
            if (!Physics.Raycast(ray, out RaycastHit hit, 20f, 1 << LayerMask.NameToLayer("ChessBoard")))
            {
                boardManager.SelectedCell = BoardInteractionManager.NOT_ON_BOARD;
                return;
            }
            
            boardManager.SelectedCell = boardManager.VisualizationManager.PositionToCell(hit.point);
        }

        
    }
}