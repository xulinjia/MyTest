/********************************************************************
	Framework Script
	Class: 	GreyFramework::SortedArray
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/
using System.Collections.Generic;


namespace GreyFramework
{
    // 插入后自动排序的数组
    public class SortedArray<T> : IEnumerable<T>
    {
        List<T> m_Items = new List<T>();
        IComparer<T> m_Comparer;

        public SortedArray(IComparer<T> comparer = null)
        {
            m_Comparer = comparer == null ? Comparer<T>.Default : comparer;
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public List<T> Items
        {
            get { return m_Items; }
        }

        public void Add(T item)
        {
            for (int i = 0; i < m_Items.Count; ++i)
            {
                if (m_Comparer.Compare(item, m_Items[i]) < 0)
                {
                    m_Items.Insert(i, item);
                    return;
                }
            }
            m_Items.Add(item);
        }

        public void Remove(T item)
        {
            m_Items.Remove(item);
        }

        public void Sort()
        {
            lock ( m_Items ) {
                m_Items.Sort(m_Comparer);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}