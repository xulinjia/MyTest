/********************************************************************
	Framework Script
	Class: 	GreyFramework::BaseElementObject
	Author:	
	Created:	
	Note:	
*********************************************************************/


namespace GreyFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class BaseElementObject
    {
        ~BaseElementObject() {
            m_Values.Clear();
        }

        #region set/get
#if true
        public void SetMember<K>(K key, object value) where K : struct
        {

            SetMember(key.GetHashCode(), SValue.FromObject(value));

        }
        public void SetMember<K>(K key, SValue value)
        {
#if UNITY_EDITOR
            if (key is int || key is long || key is ulong
                || key is uint || key is short || key is ushort || key is System.Enum)
            {
                ;
            }
            else
            {
                Debug.LogWarning("key is not vaild ,need int");
                return;
            }

#endif
            SetMember(key.GetHashCode(), value);
        }
        public void SetMember(eMember key, SValue value)
        {
            SetMember((int)key, value);
        }
        public void SetMemberEnum(eMember key, int value)
        {
            SetMemberEnum((int)key, value);
        }
        public void SetMemberEnum(int key, int value)
        {
            SetMember(key, SValue.Ctor(value));
        }

        public void SetMember(int key, SValue value)
        {
            // ProfilerModule.BeginSample(key+" 111 SetMember ");
            // ProfilerModule.BeginSample(key + " OnSetMember " + GetType());
            var flag = OnSetMember(key, ref value);
            // ProfilerModule.EndSample();
            if (flag)
            {
                // ProfilerModule.BeginSample(key+" 222 SetMember " + GetType());
                bool isChange = false;
                if (m_Values.ContainsKey(key))
                {
                    if (value == SValue.Default)
                    {
                        m_Values.Remove(key);
                        isChange = true;
                    }
                    else
                    {
                        if (m_Values[key] != value)
                        {
                            m_Values[key] = value;
                            isChange = true;
                        }
                    }
                }
                else
                {
                    // ProfilerModule.BeginSample("add value");
                    m_Values.Add(key, value);
                    isChange = true;
                    // ProfilerModule.EndSample();
                }
                // ProfilerModule.EndSample();
                //ProfilerModule.BeginSample(key+" 333 SetMember " + GetType());
                try
                {
                    if (isChange)
                    {
                        // ProfilerModule.BeginSample("onchange");
                        OnChangedMember(key, value);
                        // ProfilerModule.EndSample();
                    }

                }
                catch (System.InvalidProgramException e)
                {
                    Debug.Log(e);
                    throw;
                }
                //ProfilerModule.EndSample();
            }
            // ProfilerModule.EndSample();
        }
        public void SetMember(eMember key, object value) { SetMember((int)key, value); }
        public int GetMemberEnum(eMember key)
        { return GetMemberEnum((int)key); }
        public int GetMemberEnum(int key)
        {
            if (!m_Values.ContainsKey(key)) return 0;
            return m_Values[key].ToEnum();
        }
        public T GetMember<T,K>(K key) where T : class where K:struct
        {
#if UNITY_EDITOR
            if (key is int || key is long || key is ulong
                || key is uint || key is short || key is ushort|| key is System.Enum)
            {
                ;
            }
            else
            {
                Debug.LogWarning("key is not vaild ,need int");
                return default(T); 
            }

#endif
            //ProfilerModule.BeginSample("GetHashCode");
            int rk = key.GetHashCode();
            //ProfilerModule.EndSample();
            if (!m_Values.ContainsKey(rk)) return default(T);
            if (typeof(T) == typeof(string))
            {
                //ProfilerModule.BeginSample("Get Str");
                string str = m_Values[rk].ToString();
                //ProfilerModule.EndSample();
                //ProfilerModule.BeginSample("Get Value");
                T v = str as T;
                //ProfilerModule.EndSample();
                return v;
            }
            else
                return (T)m_Values[rk].ToObject();
        }
        public SValue GetMember(int key)
        {
            if (!m_Values.ContainsKey(key)) return SValue.Default;
            return m_Values[key];
        }
        public T GetMember<T>(int key) where T : class { return GetMember<T, int>(key); }
        public T GetMember<T>(eMember key) where T : class { return GetMember<T,int>((int)key); }
        public object GetMember(eMember key) { return GetMember((int)key); }
        public bool HasMember(int key)
        {
            return m_Values.ContainsKey(key);
        }
#else
        public void SetMember(object key, object value) 
        {
            SetMember(key, SValue.FromObject(value));
        }

        //public void SetMember<T>(object key , T value) where T : class
        //{
        //    SetMember(key , (object)value);
        //}


        public void SetMember<K>(K key , object value) 
        {
            SetMember((object)key , value);
        }
        public void SetMember<K>(K key, SValue value) 
        {
            SetMember((object)key, value);
        }
        public void SetMember<T>(eMember key, SValue value) 
        {
            SetMember((object)key, value);
        }
        public void SetMemberEnum(eMember key, int value)
        {
            SetMemberEnum((object)key, value);
        }
        public void SetMemberEnum(object key, int value)
        {
            SetMember(key, SValue.Ctor(value));
        }
        public void SetMember(object key , SValue value)
        {
            //ProfilerModule.BeginSample("SetMember "+GetType());
            if ( OnSetMember(key , ref value) ) {
                bool isChange = false;
                if ( m_Values.ContainsKey(key) )
                {
                    ProfilerModule.BeginSample("get flag");
                    var flag = value==SValue.Default;
                    ProfilerModule.EndSample();
                    if(flag)
                    {
                        m_Values.Remove( key );
                        isChange = true;
                    }
                    else
                    {
                        ProfilerModule.BeginSample("get flag 2");
                        flag = m_Values[key]!= value;
                        ProfilerModule.EndSample();
                        if (flag)
                        {
                            m_Values[key] = value;
                            isChange = true;
                        }
                    }
                }
                else 
                { 
                    ProfilerModule.BeginSample("add value");
                    m_Values.Add(key , value);
                    isChange = true;
                    ProfilerModule.EndSample();
                }
                try
                {
                    if (isChange)
                    {
                        ProfilerModule.BeginSample("onchange");
                        OnChangedMember(key, value);
                        ProfilerModule.EndSample();
                    }

                }
                catch (System.InvalidProgramException e)
                {
                    CDebug.Log(e);
                    throw;
                }
               
            }
            //ProfilerModule.EndSample();
        }

        public void SetMember(eMember key , object value) { SetMember((object)key , value); }

        public int GetMemberEnum(eMember key)
        { return GetMemberEnum((object)key); }
        public int GetMemberEnum(object key)
        {
            if (!m_Values.ContainsKey(key)) return 0;
            return m_Values[key].ToEnum();
        }
        public T GetMember<T>(object key) where T : class {
            if (!m_Values.ContainsKey(key)) return default(T);
            if (typeof(T) == typeof(string))
            {
                return (T)(object)m_Values[key].ToString();
            }
            else
                return (T)m_Values[key].ToObject();
        }
        public SValue GetMember(object key)
        {
            if ( !m_Values.ContainsKey(key) ) return SValue.Default;
            return m_Values[key];
        }

        public T GetMember<T>(eMember key) where T : class { return GetMember<T>((object)key); }

        public object GetMember(eMember key) { return GetMember((object)key); }

        public bool HasMember(object key)
        {
            return m_Values.ContainsKey(key);
        }
#endif
        #endregion

        #region extend
#if true
        protected virtual bool OnSetMember(int key, ref SValue value) { return true; }
        protected virtual void OnChangedMember(int key, SValue value)
        { }
        protected void Clear()
        {
            m_Values.Clear();
        }
#else
        protected virtual bool OnSetMember(object key, ref SValue value) { return true; }
        protected virtual void OnChangedMember(object key, SValue value)
        { }
     
#endif
#endregion
#region member
#if true
        private Dictionary<int , SValue> m_Values = new Dictionary<int , SValue>();
#else
        private Dictionary<object , SValue> m_Values = new Dictionary<object , SValue>();
#endif
#endregion

#region define
        public enum eMember
        {
            None,
            UIMemberStart,
            UIMemberMax = UIMemberStart+2000,
            Max
        }
#endregion
    }
}