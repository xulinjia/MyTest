/********************************************************************
	Framework Script
	Class: 	GreyFramework::PoolObject
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/



namespace GreyFramework
{
    public interface IPoolObject
    {
        void Initialization();
        void Reset();
        void Release();
    }

    public class PoolObject<T> where T : class, IPoolObject, new()
    {
        public int MaxCount
        {
            get { return m_MaxCount; }
            set { m_MaxCount = value; }
        }


        public PoolObject() { }
        public PoolObject(int maxCount ) { m_MaxCount = maxCount; }

        public T Create()
        {
            if (m_Pools == null)
                m_Pools = new ObjectPool<T>(m_MaxCount);

            T obj = m_Pools.New();
            obj.Initialization();
            return obj;
        }

        public void Delete(T obj)
        {
            if (m_Pools == null)
                m_Pools = new ObjectPool<T>(m_MaxCount);

            obj.Reset();
            m_Pools.Delete(obj);
        }


        private int m_MaxCount = 10;
        private ObjectPool<T> m_Pools = null;
    }
}