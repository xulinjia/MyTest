using UnityEngine;

namespace GreyFramework
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject singleGo = new GameObject(typeof(T).ToString());
                    _instance = singleGo.AddComponent<T>();
                    DontDestroyOnLoad(singleGo);
                }
                return _instance;
            }
        }
    }
}