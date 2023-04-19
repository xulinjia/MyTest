/********************************************************************
	Framework Script
	Class: 	GreyFramework::ArrayPool
	Author:	
	Created:	
	Note:	
*********************************************************************/



namespace GreyFramework
{
    using System;
    using System.Collections.Generic;

    public class ArrayPool<T>
    {
        public const int MAX_COUNT = 16;
        Queue<T[]>[] pool = new Queue<T[]>[MAX_COUNT];

        public ArrayPool()
        {
            for (int i = 0; i < MAX_COUNT; i++) {
                pool[i] = new Queue<T[]>();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < MAX_COUNT; i++) {
                if (pool[i] != null) {
                    pool[i].Clear();
                }
            }
        }

        public int NextPowerOfTwo(int v)
        {
            v -= 1;
            v |= v >> 16;
            v |= v >> 8;
            v |= v >> 4;
            v |= v >> 2;
            v |= v >> 1;
            return v + 1;
        }

        public T[] Alloc(int n)
        {
            int size = NextPowerOfTwo(n);
            int pos = GetSlot(size);

            if (pos >= 0 && pos < MAX_COUNT) {
                Queue<T[]> queue = pool[pos];
                int count = queue.Count;

                if (count > 0) {
                    return queue.Dequeue();
                }

                return new T[size];
            }

            return new T[n];
        }

        public void Collect(T[] buffer)
        {
            if (buffer == null) return;
            int pos = GetSlot(buffer.Length);

            if (pos >= 0 && pos < MAX_COUNT) {
                Queue<T[]> queue = pool[pos];
                queue.Enqueue(buffer);
            }
        }

        int GetSlot(int value)
        {
            int len = 0;

            while (value > 0) {
                ++len;
                value >>= 1;
            }

            return len;
        }
    }
}