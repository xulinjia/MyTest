using GreyFramework;
using System.Collections;

namespace ErisGame
{
    public interface IManager
    {
        public abstract bool IsCreated { get; }
        public abstract bool IsReleased { get; }
        public abstract eManager Type { get; }
        System.Collections.IEnumerator Create();

        eHRESULT Initialzation();
        void Destroy();
        void Reset();
        void Update();
        void LateUpdate();
    }
    public abstract class Manager : IManager
    {
        [System.ComponentModel.DefaultValue(false)]
        public bool IsCreated { get;private set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsReleased { get; private set; }
        [System.ComponentModel.DefaultValue(eManager.Count)]
        public abstract eManager Type { get; }

        public eHRESULT Initialzation()
        {
            IsCreated = false;
            IsReleased = false;
            if (!IsCreated)
            {
                eHRESULT state = OnInitialization();
                if (eHRESULT.Succeed != state)
                {
                    return state;
                }
                IsCreated = true;
                return eHRESULT.Succeed;
            }
            return eHRESULT.Completed;
        }
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
        public IEnumerator Create()
        {
            yield return OnCreate();
        }
        public void Update() { OnUpdate(); }
        public void Destroy()
        {
            OnDestroy();
            DisableMono();
            IsCreated = false;
            IsReleased = true;
        }

        #region
        public virtual eHRESULT OnInitialization(){ return eHRESULT.Unknow; }
        public virtual IEnumerator OnCreate() { yield return null; }
        public virtual void OnUpdate() { }
        public virtual void OnDestroy() { }

        public void Reset()
        {
            OnReset();
        }
        public virtual void OnReset() { }
        public void LateUpdate() { OnLateUpdate(); }
        protected virtual void OnLateUpdate() { }


        #endregion
        private NoneBehaviour m_Behaviour = null;
    }
}
