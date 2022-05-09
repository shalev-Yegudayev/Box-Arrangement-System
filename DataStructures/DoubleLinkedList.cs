using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public class DoubleLinkedList<T> : IEnumerable where T : IComparable<T>
    {
        public Node head;
        public Node tail;
        private int count = 0;

        public void AddFirst(T value)
        {
            Node node = new Node(value);
            if (count == 0)
            {
                head = node;
                tail = node;
            }
            else
            {
                head.Prev = node;
                node.Next = head;
                head = node;
            }
            count++;
        }
        public void AddLast(T value)
        {
            Node node = new Node(value);
            if (count == 0)
            {
                tail = head = node;
            }
            else
            {
                tail.Next = node;
                node.Prev = tail;
                tail = node;
            }
            count++;
        }

        public bool RemoveFirst(out T value)
        {
            value = head.Value;
            if (count == 0)
            {
                return false;
            }
            if (count == 1)
            {
                tail = null;
                head = null;
            }
            else
            {
                head = head.Next;
                head.Prev = null;
            }
            count--;
            return true;
        }
        public bool RemoveLast(out T value)
        {
            value = tail.Value;
            if (count == 0)
            {
                value = default;
                return false;
            }
            if (count == 1)
            {
                head = null;
                tail = null;
            }
            else
            {
                tail = tail.Prev;
                tail.Next = null;
            }
            count--;
            return true;
        }

        public bool GetAt(int index, out T value)
        {
            if (index < 0 || index >= count)
            {
                value = default;
                return false;
            }

            Node node = head;
            for (int i = 0; i < index; i++)
            {
                node = node.Next;
            }

            value = node.Value;
            return true;
        }
        public bool AddAt(int index, T value)
        {
            if (index < 0 || index > count)
            {
                return false;
            }

            if (index == 0)//want to add at index 0 - the first index
            {
                AddFirst(value);
            }
            else if (index == count)//want to add at index count - one after the largest index
            {
                AddLast(value);
            }
            else
            {
                Node node = head;
                for (int i = 0; i < index - 1; i++)
                {
                    node = node.Next;
                }

                Node nodeToAdd = new Node(value);

                node.Next.Prev = nodeToAdd;
                nodeToAdd.Next = node.Next;

                node.Next = nodeToAdd;
                nodeToAdd.Prev = node;
                count++;
            }

            return true;
        }

        public Node DisconnectNode(Node node)
        {
            if (node.Value.CompareTo(head.Value) == 0) RemoveFirst(out _);
            else
            {
                if (node.Value.CompareTo(head.Value) == 0)
                {
                    RemoveFirst(out _);
                }
                else if (node.Value.CompareTo(tail.Value) == 0)
                {
                    RemoveLast(out _);
                }
                else
                {
                    node.Prev.Next = node.Next;
                    node.Next.Prev = node.Prev;
                }
            }
            count--;
            return node;
        }
        public override string ToString()
        {
            string s = "";
            Node temp = head;
            for (int i = 0; i < count; i++)
            {
                s += temp.Value + " -> ";
                temp = temp.Next;
            }
            s += "null";

            return s;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class Node
        {
            public Node Next { get; set; }
            public T Value { get; set; }
            public Node Prev { get; set; }

            public Node(T value)
            {
                Value = value;
            }
        }
    }
}
