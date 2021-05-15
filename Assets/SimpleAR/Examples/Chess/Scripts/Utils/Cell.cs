using System;
using UnityEngine;

namespace SimpleAR.Examples.Chess.Scripts.Utils
{
    [Serializable]
    public class Cell<T>
    {
        [SerializeField] public T x;

        [SerializeField] public T y;

        public Cell(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            var o = (Cell<T>) obj;
            return x.Equals(o.x) && y.Equals(o.y);
        }
    }

    public class IntCell : Cell<int>
    {
        public IntCell(int x, int y) : base(x, y)
        {
        }
    }

    public class FloatCell : Cell<float>
    {
        public FloatCell(float x, float y) : base(x, y)
        {
        }
    }
}