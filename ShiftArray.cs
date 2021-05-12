
namespace SimpleAR
{
    public class ShiftArray<T>
    {
        private readonly T[] _arr;
        private int _index;
        private readonly int _capacity;

        public ShiftArray(int capacity)
        {
            _capacity = capacity;
            _arr = new T[capacity];
            _index = _capacity - 1;
        }
        
        public T this[int a]
        {
            get => _arr[(_index + a) % _capacity];
            set => _arr[(_index + a) % _capacity] = value;
        }

        public void Insert(T val)
        {
            _index = _index == 0 ?_capacity - 1: _index - 1;
            _arr[_index] = val;
        }
    }
}