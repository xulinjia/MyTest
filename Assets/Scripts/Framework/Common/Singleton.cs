/********************************************************************
	Framework Script
	Class: 	GreyFramework::Singleton
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/



namespace GreyFramework
{
	public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new T();
                return m_Instance;
            }
        }

        protected static T m_Instance;

        public Singleton() { }
    }
}