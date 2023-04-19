using UnityEngine;
using System;
using System.Collections.Generic;

public static class GameObjectEx
{
    public static void PrintActiveState(this GameObject gameObject)
    {
        Debug.Log("The active state of " + gameObject.name + ":" + GetActiveState(gameObject));
    }

    public static string GetActiveState(this GameObject gameObject)
    {
        string res = "";
        res += gameObject.GetPath() + ":[" + gameObject.activeSelf + "];";

        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            Transform child = gameObject.transform.GetChild(i);
            res += GetActiveState(child.gameObject);
        }
        return res;
    }


    public static GameObject Clone(this GameObject prefab)
    {
        if (null == prefab)
        {
            return null;
        }

        var cloned = GameObject.Instantiate(prefab) as GameObject;
        var prefabTransform = prefab.transform;
        cloned.transform.parent = prefabTransform.parent;
        // in NGUI, the localScale will be adjusted automatically according to the scale of camera. so restore it.
        cloned.transform.localScale = prefabTransform.localScale;
        return cloned;
    }

    public static GameObject ReplacedByPrefab(this GameObject go, GameObject prefab)
    {
        if (null == go || null == prefab)
        {
            return null;
        }

        var cloned = GameObject.Instantiate(prefab) as GameObject;
        var oldTransform = go.transform;
        cloned.transform.parent = oldTransform.parent;
        // in NGUI, the localScale will be adjusted automatically according to the scale of camera. so restore it.
        cloned.transform.localScale = oldTransform.localScale;
        GameObject.Destroy(go);
        return cloned;
    }

    public static void SetActiveRecursely(this GameObject go, bool tag)
    {
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetActiveRecursely(tag);
        }
        go.SetActive(tag);
    }

    public static void SetToLayer(this GameObject go, int layer)
    {
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetToLayer(layer);
        }

        go.layer = layer;
    }

    public static void SetParticleSystemScale(this GameObject go, float scale)
    {
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetParticleSystemScale(scale);
        }

        if (go.GetComponent<ParticleSystem>() != null)
        {
            go.GetComponent<ParticleSystem>().startSize *= scale;
            go.GetComponent<ParticleSystem>().startSpeed *= scale;
        }
    }


    public static void SetParticleSystemVisible(this GameObject go, bool bVisible)
    {
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetParticleSystemVisible(bVisible);
        }

        if (go.GetComponent<ParticleSystem>() != null)
        {
            if (bVisible)
            {
                go.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                go.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public static GameObject GetChildByName(this GameObject go, string name)
    {
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                if (child.name == name)
                {
                    return child.gameObject;
                }

                var grandson = child.gameObject.GetChildByName(name);
                if (null != grandson)
                {
                    return grandson;
                }
            }
        }

        return null;
    }

    public static GameObject GetChildHasName(this GameObject go, string name)
    {
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                if (child.name.Contains(name))
                {
                    return child.gameObject;
                }

                var grandson = child.gameObject.GetChildHasName(name);
                if (null != grandson)
                {
                    return grandson;
                }
            }
        }

        return null;
    }

    public static GameObject[] GetDirectChildren(this GameObject go, string[] names)
    {
        if (null != go && null != names)
        {
            var goes = new GameObject[names.Length];
            for (int i = 0; i < names.Length; ++i)
            {
                var name = names[i];
                for (int j = 0; j < go.transform.childCount; ++j)
                {
                    Transform child = go.transform.GetChild(j);
                    if (child.name == name)
                    {
                        goes[i] = child.gameObject;
                        break;
                    }
                }
            }

            return goes;
        }

        return null;
    }

    public static GameObject GetRootObject(this GameObject go)
    {
        if (go == null) { return null; }
        var root = go.transform;
        while (root.parent != null)
        {
            root = root.parent;
        }

        return root.gameObject;
    }

    public static IEnumerable<GameObject> GetDirectChildren(this GameObject go)
    {
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                yield return child.gameObject;
            }
        }
    }

    public static void DeleteChildren(this GameObject go)
    {
        if (go != null)
        {
            int count = go.transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject);
            }

        }
    }

    public static void DeleteChildrenImmediate(this GameObject go)
    {
        if (go != null)
        {
            int count = go.transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(go.transform.GetChild(i).gameObject, true);
            }

        }
    }


    public static string GetPath(this GameObject go)
    {
        GameObject current = go;
        string path = current.name;

        while (null != current.transform.parent)
        {
            current = current.transform.parent.gameObject;
            path = current.name + "/" + path;
        }

        return path;
    }

    public static void SetLayerRecursively(this GameObject go, string layer)
    {
        if (null == go)
            return;

        go.layer = LayerMask.NameToLayer(layer);
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetLayerRecursively(layer);
        }
    }

    public static void SetLayerRecursively(this GameObject go, int layer)
    {
        if (null == go)
            return;

        go.layer = layer;
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            child.gameObject.SetLayerRecursively(layer);
        }
    }

    public static bool GetActive(this GameObject go)
    {
        return go && go.activeInHierarchy;
    }

    public static IEnumerable<GameObject> WalkTree(this GameObject root)
    {
        if (null != root)
        {
            yield return root;
            for (int i = 0; i < root.transform.childCount; ++i)
            {
                Transform child = root.transform.GetChild(i);

                IEnumerator<GameObject> e = child.gameObject.WalkTree().GetEnumerator();

                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }
        }
    }

    public static void ReplaceMaterials(this GameObject root, Material material)
    {
        IEnumerator<GameObject> e = root.WalkTree().GetEnumerator();
        while (e.MoveNext())
        {
            GameObject child = e.Current;
            var childRenderer = child.GetComponent<Renderer>();
            if (null == childRenderer || null == childRenderer.sharedMaterials)
            {
                continue;
            }

            var materials = new Material[childRenderer.sharedMaterials.Length];
            for (int i = 0; i < materials.Length; ++i)
            {
                materials[i] = material;
            }

            childRenderer.sharedMaterials = materials;
        }
    }

    public static T SetDefaultComponent<T>(this GameObject go) where T : Component
    {
        if (null != go)
        {
            var component = go.GetComponent<T>();
            if (null == component)
            {
                component = go.AddComponent<T>();
            }

            return component;
        }

        return null;
    }

    public static T GetComponentNull<T>(this Component c, string path) where T : Component
    {
        if (c != null)
        {
            var t = c.transform;
            t = t.Find(path);
            if (t != null)
                return t.GetComponent<T>();
        }
        return null;
    }
    public static T GetComponentNull<T>(this GameObject c, string path) where T : Component
    {
        if (c != null)
        {
            var t = c.transform;
            t = t.Find(path);
            if (t != null)
                return t.GetComponent<T>();
        }
        return null;
    }

    public static T GetComponenetInParents<T>(this GameObject c) where T : Component
    {
        T com = null;
        if (c != null)
        {
            Transform t = c.transform.parent;
            while (t != null)
            {
                com = t.GetComponent<T>();
                if (com != null)
                {
                    return com;
                }
                t = t.parent;
            }
        }
        return null;
    }

    public static bool IsNull(this UnityEngine.Object o) // 或者名字叫IsDestroyed等等
    {
        return o == null;
    }
    public static void SetActiveVirtual(this GameObject go, bool bVisible)
    {
        if (bVisible != go.activeSelf)
            go.SetActive(bVisible);
    }

    public static void SetSelfActive(this GameObject go, bool isActive)
    {
        if (isActive && !go.activeSelf)
        {
            go.SetActive(true);
        }
        if (!isActive && go.activeSelf)
        {
            go.SetActive(false);
        }
    }


    public static void PlayerNameAniation(this Animator ani,string name, float layer)
    {
        ani.Play(name, (int)layer);
    }

    public static void PlayerTriggerAniation(this Animator ani, string name)
    {
        ani.SetTrigger(name);
    }

    public static void SetFloatAnimation(this Animator ani,string name,float f)
    {
        ani.SetFloat(name, f);
    }

    public static void AnimatorPlay(this Animator ani,string name)
    {
        ani.enabled = true;
        ani.Play(name);
    }

    public static void AnimatorPlayN(this Animator ani, string name,float nor_Time)
    {
        Debug.Log("AnimatorPlayN" + name + nor_Time);
        ani.Play(name,0, nor_Time);
    }

    public static float ParticleSystemLength(this GameObject obj)
    {
        ParticleSystem[] particleSystems = obj.transform.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0;
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.emission.enabled)
            {
                if (ps.main.loop)
                {
                    return 5f;
                }
                float dunration = 0f;
                if (ps.emission.rateOverTimeMultiplier <= 0)
                {
                    dunration = ps.main.startDelayMultiplier + ps.main.startLifetimeMultiplier;
                }
                else
                {
                    dunration = ps.main.startDelayMultiplier + Mathf.Max(ps.main.duration, ps.main.startLifetimeMultiplier);
                }
                if (dunration > maxDuration)
                {
                    maxDuration = dunration;
                }
            }
        }
        return maxDuration;
    }

    public static Component GetComponentEx(this Component c, string name)
    {
        if (c != null)
        {
            var n = c.GetComponent(name);
            if (n == null)
                return null;
            else
                return n;
        }
        return null;
    }
    public static Component GetComponentEx(this GameObject c, string name)
    {
        if (c != null)
        {
            var n = c.GetComponent(name);
            if (n == null)
                return null;
            else
                return n;
        }
        return null;
    }
}
