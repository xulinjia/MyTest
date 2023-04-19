/********************************************************************
	Framework Script
	Class: 	GreyFramework::BetterDictionary
	Author:	
	Created:	
	Note:	
*********************************************************************/



namespace GreyFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class BetterDictionary<TKey, TValue>
    {
        public BetterDictionary()
        {
            m_Keys = new BetterList<TKey>();
            m_Values = new BetterList<TValue>();
        }
        public BetterDictionary(int count)
        {
            m_Keys = new BetterList<TKey>(count);
            m_Values = new BetterList<TValue>(count);
        }
        public BetterList<TKey> Keys {
            get { return m_Keys; }
            private set { m_Keys = value; }
        }

        public BetterList<TValue> Values {
            get { return m_Values; }
            private set { m_Values = value; }
        }

        public int Size {
            get { return m_Keys.size; }
        }

        /// <summary>
        /// 又省又快的遍历
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TKey IndexKey(int i)
        {
            if (i < 0 || i >= m_Keys.size) {
                throw new Exception(string.Format("BetterDictionary.IndexKey {0} out of range.  {1}", i , Size));
            }

            return m_Keys[i];
        }

        public TValue IndexValue(int i)
        {
            if (i < 0 || i >= m_Values.size) {
                throw new Exception(string.Format("BetterDictionary.IndexValue {0} out of range.  {1}", i, Size));
            }

            return m_Values[i];
        }

        [DebuggerHidden]
        public TValue this[TKey i] {
            get {

                int index = m_Keys.IndexOf(i);
                if (index<0) {
                    throw new Exception("BetterDictionary key error.");
                }

                return m_Values[index];
            }
            set {

                int index = m_Keys.IndexOf(i);
                if (index < 0) {
                    Add(i, value);
                } else {
                    m_Values[index] = value;
                }
            }
        }

        public bool Add(TKey key, TValue value )
        {
            if (m_Keys.Contains(key)) {
                return false;
            }

            m_Keys.Add(key);
            m_Values.Add(value);
            return true;
        }

        public bool Remove(TKey key)
        {
            int index = m_Keys.IndexOf(key);
            if (index<0) {
                return false;
            }

            m_Keys.RemoveAt(index);
            m_Values.RemoveAt(index);
            return true;
        }

        public bool RemoveAt(int index)
        {
            m_Keys.RemoveAt(index);
            m_Values.RemoveAt(index);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int i = m_Keys.IndexOf(key);
            if (i >= 0) {
                value = m_Values[i];
                return true;
            }
            value = default(TValue);
            return false;
        }


        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            return m_Keys.IndexOf(key) >= 0;
        }

        public bool ContainsValue(TValue value)
        {
            return m_Values.IndexOf(value) >= 0;
        }

        public void Clear(int safeReleaseSize = 0)
        {
            m_Keys.Clear(safeReleaseSize);
            m_Values.Clear(safeReleaseSize);
        }

        public void Release()
        {
            m_Keys.Release();
            m_Values.Release();
        }
        private BetterList<TKey> m_Keys ;
        private BetterList<TValue> m_Values;
    }

}