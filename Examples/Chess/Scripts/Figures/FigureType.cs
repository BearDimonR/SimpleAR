using System;

namespace SimpleAR.Examples.Chess.Scripts.Figures
{
    public enum FigureType
    {
        Bishop = 3,
        King = 5,
        Queen = 4,
        Rook = 1,
        Pawn = 0,
        Knight =2
    }

    public enum FigureColour
    {
        Black = 0,
        White = 1
        
    }
    
    
    static class FigureColourUtils {
        public static bool Bool(this FigureColour value) {
            switch(value) {
                case FigureColour.Black: return false;
                case FigureColour.White: return true;
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}