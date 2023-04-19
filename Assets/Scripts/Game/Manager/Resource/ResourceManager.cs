

namespace ErisGame
{
    using GreyFramework;
    using System;

    public class ResourceManager : Manager
    {
        public override eManager Type => eManager.Resource;

        public override eHRESULT OnInitialization()
        {
            //body
            return eHRESULT.Succeed;
        }
        public void LoadSync(string path,Action<UnityEngine.Object> onResLoaded)
        {
            var loader = GetBaseLoader(path);
            loader.ExecuteSync(onResLoaded);
        }        
        public void LoadAsync(string path,Action<UnityEngine.Object> onResLoaded)
        {
            var loader = GetBaseLoader(path);
            loader.ExecuteAsync(onResLoaded);
        }

        private void Unload(string path)
        {
            BaseLoader loader = null;
            var pathLower = path.ToLower();
            if (m_Loaders.TryGetValue(pathLower, out loader))
            {
                loader.Unload();
            }
        }
        private BaseLoader GetBaseLoader(string path)
        {
            BaseLoader loader = null;
            var pathLower = path.ToLower();
            if (!m_Loaders.TryGetValue(pathLower,out loader))
            {
                if (GlobalSetting.AssetMode == eAssetMode.Editor)
                {
                    loader = new ResourceLoader(path);
                }
                else if (GlobalSetting.AssetMode == eAssetMode.Mobile)
                {

                }
                m_Loaders.Add(pathLower, loader);
            }
            return loader;
        }

        private BetterDictionary<string, BaseLoader> m_Loaders = new BetterDictionary<string, BaseLoader>();

    }
}
