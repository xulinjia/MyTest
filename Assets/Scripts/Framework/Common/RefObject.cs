/********************************************************************
	Framework Script
	Class: 	GreyFramework::RefObject
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/



namespace GreyFramework
{
    public interface IRefObject
    {
        int RefCount { get; }
        void AddRef();
        void Release();
    }

    public class RefObject : IRefObject
    {
        [System.ComponentModel.DefaultValue(false)]
        public bool IsInvalid{   get;     private set;      }

        public int RefCount { get { return m_RefCount; } }

        public virtual void AddRef()
        {
            ++m_RefCount;
            IsInvalid = false;
        }

        public virtual void Release()
        {
            if (--m_RefCount == 0)
            {
                Destroy();
                IsInvalid = true;
            }
        }

        protected virtual void Destroy() { }

        ///////////////////////////////////////
        private int m_RefCount = 0;

    }
}