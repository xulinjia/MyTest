/********************************************************************
	Framework Script
	Class: 	GreyFramework::Member
	Author:	
	Created:	
	Note:	
*********************************************************************/



using System.Collections.Generic;

namespace GreyFramework
{
    public struct Member<T>
    {
        public bool Assigned {
            get {
                return m_Change == 1;
            }
        }

        public T Value {
            get { return m_Value; }
            set {
                if (!m_Comp.Equals(m_Value,value)) {
                    m_Value = value;
                    m_Change = 1;
                }
            }
        }

        public Member(T value)
        {
            m_Change = 0;
            m_Value = value;
            m_Comp = EqualityComparer<T>.Default;
        }

        public void Reset()
        {
            m_Change = 0;
            m_Value = default(T);
        }

        private byte m_Change;
        private T m_Value;
        private EqualityComparer<T> m_Comp;

        /// <summary>
        /// 不给实现隐式的由一个T创建出一个member, 修改必须以显式的value去调用,也是为了使用者清晰
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(Member<T> value)
        {
            return value.Value;
        }

    }
}