using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            MaxHeap<int>    maxHeap = new MaxHeap<int>();
            maxHeap.Insert(100);
            maxHeap.Insert(33);
            maxHeap.Insert(66);
        }
    }
}
