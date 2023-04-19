using UnityEngine;


namespace GreyFramework
{
    public class NoneBehaviour:MonoBehaviour
    {
        static public NoneBehaviour Create(string str)
        {
            GameObject NoneGo = new GameObject(str);
            NoneGo.hideFlags = HideFlags.DontUnloadUnusedAsset | HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            NoneBehaviour _Behav = NoneGo.AddComponent<NoneBehaviour>();
            DontDestroyOnLoad(NoneGo);
            return _Behav;
        }
    }
}
