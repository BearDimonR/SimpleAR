using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts
{
    [System.Serializable]
    public class Cell<T>
    {
        [SerializeField]
        public T X;
        [SerializeField]
        public T Y;

        public Cell(T x, T y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            Cell<T> o = (Cell<T>) obj;
            return X.Equals(o.X) && Y.Equals(o.Y);
        }
    }
    
    public class IntCell: Cell<int>
    {
        public IntCell(int x, int y) : base(x, y) { }
    }

    public class FloatCell : Cell<float>
    {
        public FloatCell(float x, float y) : base(x, y) { }
    }
}