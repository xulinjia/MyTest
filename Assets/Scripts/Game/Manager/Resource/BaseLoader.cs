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





        protected string m_Path;
        protected Type m_Type;
        protected UnityEngine.Object m_Asset;
        protected eLoadState m_LoadState = eLoadState.Invalid;
        protected int m_refCount;
        protected List<Action<UnityEngine.Object>> m_OnLoadCalls = new List<Action<UnityEngine.Object>>();
    }
}
