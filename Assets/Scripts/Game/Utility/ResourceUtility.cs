
namespace ErisGame
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ResourceUtility
    {
        static public Type ExtToType(string ext)
        {
            Type type = null;
            m_assetTypeDic.TryGetValue(ext, out type);
            return type;
        }

        static public string TypeToExt(Type type)
        {
            foreach (var item in m_assetTypeDic)
            {
                if (item.Key.Equals(type))
                {
                    return item.Key;
                }
            }
            return null;
        }

        private static Dictionary<string, Type> m_assetTypeDic = new Dictionary<string, Type>()
        {
            {".prefab",typeof(GameObject)},

        };
    }
}
