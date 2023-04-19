using GreyFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErisGame
{

    public sealed class Managers
    {
        static public ResourceManager Recource
        {
            get
            {
                return GameClient.Manager<ResourceManager>(eManager.Resource);
            }
        }

        static public ResourceManager GetResourceManager()
        {
            return Recource;
        }
    }

    static public class GameClient
    {
        [System.ComponentModel.DefaultValue(false)]
        static public bool IsCreated { get; private set; }

        static public eHRESULT InitGame(CallBack<eHRESULT> _createResult)
        {
            if (null == m_behaviour)
            {
                m_behaviour = NoneBehaviour.Create("GameClient");
            }

            m_createCall = _createResult;
            m_behaviour.StartCoroutine(OnCreate());
            for (eManager i = 0; i < eManager.Count; i++)
            {
                IManager mod = ManagerDefine.CreateInstance(i) as IManager;
                Register(ref mod);
            }
            m_behaviour.StartCoroutine(OnCreate());
            return eHRESULT.Completed;
        }
        static private IEnumerator OnCreate()
        {
            for (int i = 0; i < m_managers.Count; i++)
            {
                var item = m_managers[i];
                yield return item.Create();
            }

            bool _result = true;
            for (int i = 0; i < m_managers.Count; i++)
            {
                if (!m_managers[i].IsCreated)
                {
                    _result = false;
                    UnityEngine.Debug.LogFormat("No IsCreated : {0}", m_managers[i].GetType());
                }
            }

            yield return null;
            IsCreated = true;
            if (m_createCall != null)
            {
                m_createCall.Invoke(_result ? eHRESULT.Succeed : eHRESULT.Failed);
                m_createCall = null;
            }
            yield return null;
        }
        static public void Register(ref IManager model)
        {
            if (null == model) return;
            UnRegister(ref model);
            m_managerTypes.Add(model.Type);
            m_managers.Add(model);
            model.Initialzation();
        }
        static public void UnRegister(ref IManager model)
        {
            if (null == model) return;
            eManager type = model.Type;
            for (int i = 0; i < m_managerTypes.Count; i++)
            {
                if (m_managerTypes[i].Equals(type))
                {
                    m_managers[i].Destroy();
                    m_managers.RemoveAt(i);
                    m_managerTypes.RemoveAt(i);
                    break;
                }
            }
        }
        static public void Update()
        {
            if (!IsCreated) return;
            for (int i = 0; i < m_managers.Count; i++)
            {
                IManager manager = m_managers[i];
                if (manager.IsCreated)
                {
                    manager.Update();
                }
            }
        }

        static public void LateUpdate()
        {
            if (!IsCreated) return;
            for (int i = 0; i < m_managers.Count; i++)
            {
                IManager manager = m_managers[i];
                if (manager.IsCreated)
                {
                    manager.LateUpdate();
                }
            }
        }

        static public void Destroy()
        {
            if (null != m_behaviour)
            {
                UnityEngine.GameObject go = m_behaviour.gameObject;
                UnityEngine.Object.Destroy(m_behaviour);
                if (null != go)
                {
                    UnityEngine.Object.Destroy(go);
                }
                m_behaviour = null;
            }

            for (int i = m_managers.Count - 1; i >= 0; i--)
            {
                IManager _module = m_managers[i];
                if (_module != null && _module.IsCreated)
                {
                    _module.Destroy();
                }
            }

        }
        static public T Manager<T>(eManager type) where T : IManager
        {
            for (int i = 0; i < m_managerTypes.Count; i++)
            {
                if (m_managerTypes[i] == type)
                {
                    return (T)m_managers[i];
                }
            }
            return default(T);
        }
        static private List<IManager> m_managers = new List<IManager>((int)eManager.Count);
        static private NoneBehaviour m_behaviour = null;
        static private CallBack<eHRESULT> m_createCall;
        static private List<eManager> m_managerTypes = new List<eManager>((int)eManager.Count);

    }
}
