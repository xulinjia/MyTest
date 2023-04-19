using GreyFramework;
using System.Collections;

namespace ErisGame
{
    public interface IManager
    {
        [System.ComponentModel.DefaultValue(false)]
        bool IsCreated { get; }
        [System.ComponentModel.DefaultValue(false)]
        bool IsRelease { get; }

        [System.ComponentModel.DefaultValue(eManager.Count)]
        eManager Type { get; }

        eHRESULT Initialization();
        System.Collections.IEnumerator Create();
        void Destroy();
        void Reset();
        void Update();
        void LateUpdate();
    }

    public abstract class Manager : IManager
    {
        [System.ComponentModel.DefaultValue(false)]
        public bool IsCreated { get; private set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool IsRelease { get; private set; }

        [System.ComponentModel.DefaultValue(eManager.Count)]
        public abstract eManager Type { get; }

        protected NoneBehaviour Behaviour { get { return m_Behaviour; } }

        #region INIT

        public eHRESULT Initialization()
        {
            IsCreated = false;
            if (eHRESULT.Succeed == OnInitialization())
            {
                IsCreated = true;
                return eHRESULT.Succeed;
            }

            return eHRESULT.Unknow;
        }

        public virtual eHRESULT OnInitialization() { return eHRESULT.Unknow; }

        public System.Collections.IEnumerator Create()
        {
            yield return OnCreate();
        }

        public virtual System.Collections.IEnumerator OnCreate()
        {
            yield return null;
        }

        #endregion

        #region DESTROY
        public void Destroy()
        {
            OnDestroy();
            DisableMono();
            IsCreated = false;
            IsRelease = true;
        }

        public virtual void OnDestroy() { }
        #endregion

        #region FUN
        public void Update() { OnUpdate(); }

        protected virtual void OnUpdate() { }

        public void LateUpdate() { OnLateUpdate(); }
        protected virtual void OnLateUpdate() { }

        public void Reset()
        {
            OnReset();
        }

        public virtual void OnReset() { }

        protected void EnableMono()
        {
            if (null == m_Behaviour)
            {
                m_Behaviour = NoneBehaviour.Create(Type.ToString());
            }
        }

        protected void DisableMono()
        {
            if (null != m_Behaviour)
            {
                UnityEngine.GameObject go = m_Behaviour.gameObject;
                UnityEngine.Object.Destroy(m_Behaviour);
                if (null != go)
                {
                    UnityEngine.Object.Destroy(go);
                }
                m_Behaviour = null;
            }
        }
        #endregion

        //////////////////////////////////////////////////////
        private NoneBehaviour m_Behaviour = null;
    }
}
