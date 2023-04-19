/********************************************************************
	Framework Script
	Class: 	GreyFramework::ObjectPool
	Author:	
	Created:	
	Note:	$end$
*********************************************************************/





namespace GreyFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPool<T> where T : class, new()
    {
        public event CallBackRef<T> onActionOnDestory = delegate { };


        public ObjectPool()
        {
            m_Stack = new BetterList<T>();
        }

        public int GetCurCount() {
            return m_Stack.size;
        }

        public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease, int maxCount=50)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_MaxCount = maxCount;
            m_Stack = new BetterList<T>();
        }

        public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease, CallBackRef<T> actionOnDestory ,int maxCount = 50)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_MaxCount = maxCount;
            onActionOnDestory += actionOnDestory;
            m_Stack = new BetterList<T>();
        }

        public ObjectPool(int maxCount)
        {
            m_MaxCount = maxCount;
            m_Stack = new BetterList<T>();
        }

        public T New()
        {
            T obj = null;
            if (m_Stack.size>0 && m_Stack.size <= m_MaxCount)
            {
                obj = m_Stack.Pop();
            }else{
                obj = new T();
            }

            if (null != m_ActionOnGet){
                m_ActionOnGet.Invoke(obj);
            }
            
            return obj;
        }

        public void Delete(T obj)
        {
            if (obj == null)  return ;
             if (m_Stack.size > 0 && ReferenceEquals(m_Stack.Peek(), obj)){
                throw (new System.Exception("Internal error. Trying to destroy object that is already released to pool."));
            }

            if (null != m_ActionOnRelease){
                m_ActionOnRelease.Invoke(obj);
            }

            if (m_Stack.size < m_MaxCount)
                m_Stack.Add(obj);
            else
            {
                if (onActionOnDestory != null)
                {
                    onActionOnDestory.Invoke(ref obj );
                }
                obj = null;
            }
        }


        private int m_MaxCount = 5;
        private int m_Count = 0;
        private readonly BetterList<T> m_Stack;
        private readonly Action<T> m_ActionOnGet = null;
        private readonly Action<T> m_ActionOnRelease = null;
    }


    public class ObjectPoolEx<T>
    {
        public delegate T OnCreateObject();

        public ObjectPoolEx(OnCreateObject actionOnCreate, Action<T> actionOnReset, int maxCount = 50 , CallBackRef<T> actionOnRelease=null)
        {
            if (actionOnCreate == null)
                Debug.LogWarning("actionOnCreate is null");
            m_ActionOnCreate = actionOnCreate;
            m_ActionOnReset = actionOnReset;
            m_ActionOnRelease = actionOnRelease;
            m_Datas = new BetterList<T>();
            m_MaxCount = maxCount;
        }
        public void AllocMaxCount(){
            for (int i = 0; i < m_MaxCount; i++)
            {
                if (null != m_ActionOnCreate)
                {
                    var obj = m_ActionOnCreate();
                    if (m_Datas.size < m_MaxCount)
                    {
                        m_Datas.Add(obj);
                    }
                    else
                    {
                        Debug.LogWarning("size out");
                        break;
                    }
                }
            }
        }

        public T New()
        {
            T obj = default(T);
            if (m_Datas.size > 0 )
            {
                obj = m_Datas.Pop();
            }
            else
            {
                if (null != m_ActionOnCreate)
                {
                    obj = m_ActionOnCreate();
                }
            }

            return obj;
        }

        public void Delete(T obj)
        {
            if (obj == null) return;

            if (null != m_ActionOnReset)
            {
                m_ActionOnReset.Invoke(obj);
            }

            if (m_Datas.size < m_MaxCount) {
                m_Datas.Add(obj);
            } else {
                if (m_ActionOnRelease != null) {
                    m_ActionOnRelease(ref obj);
                } else {
                    obj = default(T);
                }
            }
        }
        public void Clear(){
            m_Datas.Release();
        }


        private int m_MaxCount = 5;
        private int m_Count = 0;
        private readonly BetterList<T> m_Datas;
        private readonly OnCreateObject m_ActionOnCreate = null;
        private readonly Action<T> m_ActionOnReset = null;
        private readonly CallBackRef<T> m_ActionOnRelease = null;

    }

    public class ObjectPoolDynamic<T> where T : class, new()
    {
        public delegate T OnCreateObject( Type type );

        public ObjectPoolDynamic( OnCreateObject actionOnCreate , Action<T> actionOnReset , int maxCount = 50 )
        {
            m_ActionOnCreate = actionOnCreate;
            m_ActionOnReset = actionOnReset;
            m_Stack = new Dictionary<System.Type , BetterList<T>>();
        }


        public V New<V>() where V :  class, T
        {
            m_OperateType = typeof( V );
            BetterList<T> objs;
            V obj = default(V);
            if (m_Stack.Count > 0 && m_Stack.TryGetValue( m_OperateType , out objs ) && objs.size > 0 )
            {
                obj = objs.Pop() as V;
            }
            else
            {
                if (null != m_ActionOnCreate)
                {
                    obj = m_ActionOnCreate(m_OperateType) as V;
                }
            }


            return obj;
        }

        public bool Delete<V>( V obj ) where V : class, T {

            if (obj == null) return false;
            m_OperateType = typeof( V );

            BetterList<T> objs;
            if (m_Stack.TryGetValue( m_OperateType , out objs )) {

                if (null != m_ActionOnReset)
                {
                    m_ActionOnReset.Invoke(obj);
                }

                if (objs.size > m_MaxCount) {
                    objs = null;
                    return true;
                }

                if (objs.size > 0 && ReferenceEquals( objs.Peek() , obj )) {
                    throw ( new System.Exception( string.Format( "{0} : Internal error. Trying to destroy object that is already released to pool." , obj.GetType() ) ) );
                }

                objs.Add( obj );
            }
            else {
                objs = new BetterList<T>();
                if (null != m_ActionOnReset) {
                    m_ActionOnReset.Invoke( obj );
                }

                objs.Add( obj );
                m_Stack.Add( m_OperateType , objs );
            }

            return true;
        }


        private int m_MaxCount = 50;
        private int m_Count = 0;
        private readonly Dictionary<System.Type, BetterList<T>> m_Stack;
        private readonly OnCreateObject m_ActionOnCreate = null;
        private readonly Action<T> m_ActionOnReset = null;
        private Type m_OperateType = null;
    }
}