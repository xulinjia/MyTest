/********************************************************************
	Framework Script
	Class: 	GreyFramework::MemberEx
	Author:	
	Created:	
	Note:	
*********************************************************************/



using System.Collections.Generic;

namespace GreyFramework
{
    public struct MemberEx<T , N>
    {
        public delegate void CallBack(N number ,ref T _call1);

        public bool Assigned {
            get {
                return m_Change == 1;
            }
        }

        public bool HasOriginal {
            get {
                return m_HasOriginal == 1;
            }
        }

        public N Number {
            get { return m_Number; }
        }

        public T Value {
            get { return m_Value; }
            set {
                SetValue(ref value);
            }
        }

        public T OriginalValue {
            get { return m_OriginalValue; }
        }
        

        public void SetValue( ref T value , bool force=false)
        {
            if (!m_Comp.Equals(m_Value, value) || m_Change == 0 || force ) {
                m_Change = 1;
                if (OnMemberChange != null) {
                    OnMemberChange.Invoke(m_Number, ref value);
                }
                m_Value = value;
            }
        }

        public void SetMember(N value)
        {
            m_Number = value;
        }

        public void CollectValue(ref T value)
        {
            if (!m_Comp.Equals(m_Value, value) || m_Change == 0) {
                m_Change = 1;
                m_Value = value;
            }

            m_OriginalValue = m_Value;
            m_HasOriginal = 1;
        }

        public void Notify(ref T value)
        {
            m_Change = 1;
            if (OnMemberChange != null)
            {
                OnMemberChange.Invoke(m_Number, ref value);
            }
            m_Value = value;
        }

        public MemberEx(N member ,T value , CallBack call=null )
        {
            m_Comp = EqualityComparer<T>.Default;
            m_Change = 0;
            m_Number = member;
            m_Value = value;
            m_OriginalValue = value;
            m_HasOriginal = 1;
            OnMemberChange = call;
        }

        public MemberEx(N member, CallBack call = null)
        {
            m_Comp = EqualityComparer<T>.Default;
            m_Change = 0;
            m_Number = member;
            m_Value = default(T);
            m_OriginalValue = default(T);
            m_HasOriginal = 0;
            OnMemberChange = call;
        }

        public void Reset()
        {
            OnMemberChange = null;
            m_HasOriginal = 0;
            m_Change = 0;
            m_Number = default(N);
            m_Value = default(T);
            m_OriginalValue = default(T);
        }

        public void Clear()
        {
            m_Change = 0;
            m_HasOriginal = 0;
            m_Value = default(T);
        }

        public void ResetModifyState()
        {
            m_Change = 0;
        }

        public void SetChangeCall(CallBack memberChange)
        {
            OnMemberChange = memberChange;
        }

        public void SetOriginalValue(ref T value)
        {
            m_OriginalValue = value;
            m_HasOriginal = 1;
        }

        private CallBack OnMemberChange;
        private N m_Number;
        private byte m_Change;
        private T m_Value;
        private EqualityComparer<T> m_Comp;
        private T m_OriginalValue;
        private byte m_HasOriginal;
        /// <summary>
        /// 不给实现隐式的由一个T创建出一个member, 修改必须以显式的value去调用,也是为了使用者清晰
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(MemberEx<T, N> value)
        {
            return value.Value;
        }
    }
}