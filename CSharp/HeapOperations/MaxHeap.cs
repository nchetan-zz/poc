using System;

namespace HeapOperations
{
    public class MaxHeap<T> where T: class, IComparable<T>
    {
        private HeapNode<T> _root = null;
        private int _nodeCount = 0;

        public void Insert(T newValue)
        {
            ++_nodeCount;
            if (_root == null)
            {
                _root = new HeapNode<T>(newValue);
                return;
            }

            var newNode = new HeapNode<T>(newValue);
            var startParentNode = _GetParentNode();
            newNode.Parent = startParentNode;
            if (startParentNode.Left == null)
                startParentNode.Left = newNode;
            else if (startParentNode.Right == null)
                startParentNode.Right = newNode;
            else
                throw new ApplicationException("Something is not right!. Parent node has both filled.");

            _Reheapify(newNode);
        }

        private void _Reheapify(HeapNode<T> newNode)
        {
            if (newNode == _root) return;
            var parentNode = newNode.Parent;

            if (newNode.Parent == _root)
            {
                _root = newNode;
                return;
            }

            while ( (newNode.Value.CompareTo(newNode.Parent.Value) > 0)
                     && (newNode != _root))
            {
                if (parentNode.Left == newNode)
                {
                    newNode.Left = parentNode;
                    parentNode.Left = null;
                }
                else
                {
                    newNode.Right = parentNode;
                    parentNode.Right = null;
                }
                var tempGrandParent = parentNode.Parent;
                parentNode.Parent = newNode;
                newNode.Parent = tempGrandParent;
            }
        }

        private HeapNode<T> _GetParentNode()
        {
            if (_nodeCount < 3)
                return _root;

            var nodeNumber = _nodeCount+1;
            var treeHeight = (int) Math.Log(nodeNumber);
            var fullNodesInTree = (1 << treeHeight+1) - 1;

            var parentNode = _root;
            for (var currentHeight = 1; currentHeight < treeHeight; ++currentHeight)
            {
                var nodesAtCurrentHeight = 1 << currentHeight;
                var remainingNodes = nodesAtCurrentHeight - (fullNodesInTree - nodeNumber);
                var halfNodesAtCurrentLevel = nodesAtCurrentHeight/2;
                parentNode = remainingNodes <= halfNodesAtCurrentLevel ? parentNode.Left : parentNode.Right;
            }

            return parentNode;
        }
    }
}
