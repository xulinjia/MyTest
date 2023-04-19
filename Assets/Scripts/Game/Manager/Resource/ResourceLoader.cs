namespace ErisGame
{
    using GreyFramework;
    using System;
    using System.Collections;
    using System.IO;
    using UnityEngine;

    class ResourceLoader : BaseLoader
    {

        public ResourceLoader(string path)
        {
            m_Path = path;
            var ext = Path.GetExtension(path);
            m_Type = ResourceUtility.ExtToType(ext);
        }
        public override void ExecuteSync(Action<UnityEngine.Object> onResLoaded)
        {
            m_OnLoadCalls.Add(onResLoaded);
            if (m_LoadState == eLoadState.LoadComplete)
            {
                onLoadedCallback();
            }
            else
            {
                if (m_LoadState == eLoadState.Loading)
                {
                    StopCoroutine();
                }
                LoadSync();
            }
        }
        private void LoadSync()
        {
            m_LoadState = eLoadState.Loading;
            string pathNoExt = m_Path.Replace(Path.GetExtension(m_Path),"");
            m_Asset = Resources.Load(pathNoExt, m_Type);
            m_LoadState = eLoadState.LoadComplete;
            onLoadedCallback();
        }

        public override void ExecuteAsync(Action<UnityEngine.Object> onResLoaded)
        {
            m_OnLoadCalls.Add(onResLoaded);
            if (m_LoadState == eLoadState.LoadComplete)
            {
                //此处存疑
                onLoadedCallback();
            }
            else
            {
                if (m_LoadState == eLoadState.Loading)
                {
                    return;
                }
                CoroutineUtility.Instance.StartCoroutine(LoadAsync());
            }
        }
        private IEnumerator LoadAsync()
        {
            m_LoadState = eLoadState.Loading;
            string p = m_Path.Replace(Path.GetExtension(m_Path), "");
            ResourceRequest resq = Resources.LoadAsync(p, m_Type);
            while (resq.isDone != true)
            {
                yield return null;
            }
            m_Asset = resq.asset;
            m_LoadState = eLoadState.LoadComplete;
            onLoadedCallback();
        }
        //回调执行完后，自动清空
        private void onLoadedCallback()
        {
            m_refCount++;
            if (m_Asset == null)
            {
                Debug.LogError("Asset is null:" + m_Path);
                return;
            }

            foreach (var callback in m_OnLoadCalls)
            {
                callback(m_Asset);
            }
            m_OnLoadCalls.Clear();
        }

        public void StopCoroutine()
        {
            CoroutineUtility.Instance.StopCoroutine(m_Coroutine);
        }
        public override void Unload()
        {
            m_refCount--;
            if (m_refCount<0)
            {
                Debug.LogError(m_Asset.name + " refCount:" + m_refCount);
            }
        }
        public override void UnloadAsset()
        {
            if (m_Asset == null)
            {
                Debug.LogError("Asset is null");
                return;
            }
            if (m_Type == typeof(GameObject))
            {
                m_Asset = null;
                Resources.UnloadUnusedAssets();
            }
            else
            {
                Resources.UnloadAsset(m_Asset);
                m_Asset = null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            m_Coroutine = null;

        }
        private Coroutine m_Coroutine = null;
    }
}
