/********************************************************************
	Framework Script
	Class: 	GreyFramework::WeakObject
	Author:	
	Created:	
	Note:	 弱引用对象,局部传参或需要时, 使用Target
*********************************************************************/





namespace GreyFramework
{
    using System.Runtime.Serialization;
    public class WeakObject<T> : System.WeakReference where T : class
    {
        public WeakObject( T target ) : base(target)
        { }

        public WeakObject( T target , bool trackResurrection ) : base(target , trackResurrection)
        { }

        protected WeakObject( SerializationInfo info , StreamingContext context )
        : base(info , context)
        { }

        public new T Target
        {
            get { return base.Target== null ? null : ( T)base.Target; }
            set { base.Target = value; }
        }
    }

}