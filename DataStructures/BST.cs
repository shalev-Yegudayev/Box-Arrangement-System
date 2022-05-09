using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class BST<T> where T : IComparable<T>
    {
        Node root;

        public void Add(T item) // O(logN)
        {
            if (root == null)
            {
                root = new Node(item);
                return;
            }

            Node tmp = root;
            while (true)
            {
                if (item.CompareTo(tmp.Value) < 0) // item < tmp.value - go left
                {
                    if (tmp.Left == null)
                    {
                        tmp.Left = new Node(item);
                        break;
                    }
                    else tmp = tmp.Left;
                }
                else // go right
                {
                    if (tmp.Right == null)
                    {
                        tmp.Right = new Node(item);
                        break;
                    }
                    else tmp = tmp.Right;
                }
            }
        }

        public bool GetCorrectNode(T item, out T foundItem)
        {
            Node notFound = new Node(default);
            Node tmp = root;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.Value) == 0)
                {
                    if (tmp.Right != null)
                    {
                        foundItem = tmp.Right.Value;
                        return true;
                    }
                    foundItem = notFound.Value;
                    return false;
                }
                else if (item.CompareTo(tmp.Value) < 0) tmp = tmp.Left;

                else tmp = tmp.Right;
            }
            foundItem = notFound.Value;
            return false;
        }

        public bool Search(T item, out T foundItem)
        {
            Node tmp = root;
            Node notFound = new Node(default);
            while (tmp != null)
            {
                if (item.CompareTo(tmp.Value) == 0)
                {
                    foundItem = tmp.Value;
                    return true;
                }
                else if (item.CompareTo(tmp.Value) < 0) tmp = tmp.Left;

                else tmp = tmp.Right;
            }
            foundItem = notFound.Value;
            return false;
        }

        public int GetLevelsCnt() => GetLevelsCnt(root);

        int GetLevelsCnt(Node subTreeRoot)
        {
            if (subTreeRoot == null) return 0;

            int leftTreeDepth = GetLevelsCnt(subTreeRoot.Left);
            int rightTreeDepth = GetLevelsCnt(subTreeRoot.Right);

            return Math.Max(leftTreeDepth, rightTreeDepth) + 1;
        }

        public void ScanInOrder(Action<T> singleItemAction) => ScanInOrder(root, singleItemAction);

        void ScanInOrder(Node subTreeRoot, Action<T> singleItemAction)
        {
            if (subTreeRoot == null) return;

            ScanInOrder(subTreeRoot.Left, singleItemAction);
            singleItemAction(subTreeRoot.Value); //invoke
            ScanInOrder(subTreeRoot.Right, singleItemAction);
        }

        #region Remove
        public bool Remove(T item, out T foundItem)
        {
            Search(item, out T item1);
            Node node = new Node(item1);
            foundItem = default;
            if (node != null)
            {
                if (Remove(node, out foundItem))
                {
                    return true;
                }
            }
            return false;
        }

        private bool Remove(Node item, out T foundItem)
        {
            Node tmp = root;
            Node tmpAba = null;
            if (!Search(item.Value, out _)) { foundItem = default; return false; }
            while (tmp != null)
            {
                if (tmp.Value.CompareTo(item.Value) == 0)
                {
                    if (tmp.Right == null && tmp.Left == null)
                        return IsLeaf(tmp, tmpAba, out foundItem);

                    if (tmp.Right != null && tmp.Left == null)
                        return IsOneChild(tmp, tmpAba, tmp.Right, out foundItem);

                    if (tmp.Right == null && tmp.Left != null)
                        return IsOneChild(tmp, tmpAba, tmp.Left, out foundItem);

                    if (tmp.Right != null && tmp.Left != null)
                        return IsTwoChildren(tmp, out foundItem);
                }
                tmpAba = tmp;
                if (item.Value.CompareTo(tmp.Value) < 0) tmp = tmp.Left;
                else tmp = tmp.Right;
            }
            foundItem = default;
            return false;
        }

        private bool IsLeaf(Node tmp, Node tmpAba, out T foundItem)
        {
            foundItem = tmp.Value;
            if (tmpAba != null)
            {
                if (tmpAba.Right == tmp) tmpAba.Right = null;
                if (tmpAba.Left == tmp) tmpAba.Left = null;
            }
            else root = null;
            return true;
        }

        private bool IsOneChild(Node tmp, Node tmpAba, Node tmpSide, out T foundItem)
        {
            foundItem = tmp.Value;
            if (tmpAba != null)
            {
                if (tmpAba.Right == tmp) tmpAba.Right = tmpSide;
                if (tmpAba.Left == tmp) tmpAba.Left = tmpSide;
            }
            else
            {
                root = tmpSide;
            }
            return true;
        }

        private bool IsTwoChildren(Node tmp, out T foundItem)
        {
            Node tmp2 = tmp.Right;
            foundItem = tmp.Value;
            while (tmp2.Left != null)
            {
                tmp2 = tmp2.Left;
            }
            Remove(tmp2, out foundItem);
            tmp.Value = foundItem;
            return true;
        }
        #endregion

        #region FindClosestNode
        public bool FindClosetBiggerNotEqual(T item, out T foundItem)
        {
            Node tmp = root;
            if (root == null)
            {
                foundItem = default;
                return false;
            }

            if (item.CompareTo(root.Value) >= 0)
            {
                if (tmp.Right == null)
                {
                    foundItem = default;
                    return false;
                }
                ScanInOrderRight(tmp.Right, item, out foundItem);
                if (foundItem != null) return true;
                return false;
            }
            else
            {
                if (tmp.Left == null)
                {
                    foundItem = tmp.Value;
                    return true;
                }
                tmp.Left.Prev = tmp;
                ScanInOrderLeft(tmp.Left, item, out foundItem);
                if (foundItem != null) return true;
                return false;
            }
        }
        void ScanInOrderRight(Node subTreeRot, T item, out T foundItem)
        {
            foundItem = default;
            if (subTreeRot == null)
            {
                if (root.Value.CompareTo(item) > 0)
                {
                    foundItem = root.Value;
                    return;
                }
                return;
            }
            if (subTreeRot.Value.CompareTo(item) > 0)
            {
                Node tmp = subTreeRot;
                while (tmp.Left != null && tmp.Left.Value.CompareTo(item) > 0)
                {
                    tmp = tmp.Left;
                }
                foundItem = tmp.Value;
                return;
            }
            ScanInOrderRight(subTreeRot.Right, item, out foundItem);
        }
        void ScanInOrderLeft(Node subTreeRot, T item, out T foundItem)
        {
            foundItem = default;
            if (subTreeRot == null) return;
            if (subTreeRot.Value.CompareTo(item) <= 0)
            {
                if (subTreeRot.Right != null)
                    ScanInOrderRight(subTreeRot.Right, item, out foundItem);
                else
                    foundItem = subTreeRot.Prev.Value;
                return;
            }
            if (subTreeRot.Left != null)
            {
                subTreeRot.Left.Prev = subTreeRot;
                ScanInOrderLeft(subTreeRot.Left, item, out foundItem);
            }
            return;
        }
        #endregion

        public class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public Node Prev;

            public Node(T value)
            {
                this.Value = value;
                Left = Right = null;
            }
        }
    }
}
