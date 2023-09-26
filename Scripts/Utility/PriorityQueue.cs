using System.Collections.Generic;
using System;

namespace ManaSword.Utility
{
    public class PriorityQueue<T>
    {
        private List<Tuple<T, int, int>> elements = new List<Tuple<T, int, int>>();
        private int uniqueId = 0;

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, int priority)
        {
            elements.Add(new Tuple<T, int, int>(item, priority, uniqueId++));
            elements.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        }

        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("우선순위 큐가 비어 있습니다.");
            }

            T item = elements[0].Item1;
            elements.RemoveAt(0);
            return item;
        }

        public bool IsEmpty()
        {
            return elements.Count == 0;
        }
    }
}
