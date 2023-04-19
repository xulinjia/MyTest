using System;
using System.Collections.Generic;
using UnityEngine;

namespace GreyFramework
{
    internal  class ListPool<T>
    {
        // Object pool to avoid allocations.
        private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, Reset, 20);

        public static List<T> Get()
        {
            return s_ListPool.New();
        }

        public static void Release(List<T> toRelease)
        {
            s_ListPool.Delete(toRelease);
        }

        static void Reset(List<T> obj)
        {
            obj.Clear();
        }
    }
}
