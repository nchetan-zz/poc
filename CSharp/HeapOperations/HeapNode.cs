using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapOperations
{

    class HeapNode<T> where T  :class
    {
        public T Value { get; set; }
        public HeapNode<T> Left { get; set; }
        public HeapNode<T> Right { get; set; }
        public HeapNode<T> Parent { get; set; }

        public HeapNode(T value)
        {
            Value = value;
            Left = Right = Parent = null;
        }
    }
}
