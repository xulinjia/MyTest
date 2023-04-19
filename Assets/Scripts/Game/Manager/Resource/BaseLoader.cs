namespace ErisGame
{
    using System;
    using System.Collections.Generic;
    using GreyFramework;
    public abstract class BaseLoader
    {
        public abstract void ExecuteSync(Action<UnityEngine.Object> onResLoaded);
        public abstract void ExecuteAsync(Action<UnityEngine.Object> onResLoaded);

        public abstract void Unload();

        public virtual bool CheckUnload()
        {
            if (m_LoadState == eLoadState.LoadComplete && m_refCount <= 0)
            {
                UnloadAsset();
                m_LoadState = eLoadState.Unloaded;
                return true;
            }
            return false;
        }

        public abstract void UnloadAsset();

        public virtual void Dispose()
        {
            m_Type = null;
            m_Path = null;
            m_Asset = null;
            m_LoadState = eLoadState.Invalid;
            m_refCount = 0;
            m_OnLoadCalls = null;
        }


        protected string m_Path;
        protected Type m_Type;
        protected UnityEngine.Object m_Asset;
        protected eLoadState m_LoadState = eLoadState.Invalid;
        protected int m_refCount;
        protected List<Action<UnityEngine.Object>> m_OnLoadCalls = new List<Action<UnityEngine.Object>>();
    }
}
