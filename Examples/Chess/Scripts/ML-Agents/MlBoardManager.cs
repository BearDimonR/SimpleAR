using System.Collections.Generic;

namespace SimpleAR.Examples.Chess.Scripts
{
    public class MlBoardManager: BoardManager
    {
        public PlayerAgent whiteAgent;
        public PlayerAgent blackAgent;

        public new void Awake()
        {
            base.Awake();
            whiteAgent.isWhite = true;
            blackAgent.isWhite = false;
        }
        
        public void UpdateFigures(ref float[] figures)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateMoves(ref List<float[]> moves)
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            if (isWhiteTurn)
            {
                whiteAgent.RequestDecision();
            }
            else
                blackAgent.RequestDecision();
        }
    }
}