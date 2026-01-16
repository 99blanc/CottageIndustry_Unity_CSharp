using Cysharp.Text;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtensions
{
    public static T FindChild<T>(this GameObject gameObject, string name = null, bool recursive = false) where T : Object
    {
        if (!gameObject)
            throw new InvalidOperationException();

        if (recursive)
        {
            T[] caches = gameObject.GetComponentsInChildren<T>(true);

            for (int index = 0; index < caches.Length; ++index)
            {
                if (string.IsNullOrEmpty(name) || ZString.Equals(name, caches[index].name))
                    return caches[index];
            }

            throw new InvalidOperationException();
        }

        for (int index = 0; index < gameObject.transform.childCount; ++index)
        {
            Transform child = gameObject.transform.GetChild(index);

            if (!string.IsNullOrEmpty(name) && !ZString.Equals(name, child.name))
                continue;

            if (child.TryGetComponent<T>(out T comp))
                return comp;
        }

        throw new InvalidOperationException();
    }

    public static GameObject FindChild(this GameObject gameObject, string name = null, bool recursive = false) => FindChild<Transform>(gameObject, name, recursive).gameObject;

    public static Transform FindAssert(this Transform transform, string name)
    {
        Transform newTransform = transform.Find(name);
        Debug.Assert(newTransform);

        return newTransform;
    }

    public static T GetComponentAssert<T>(this GameObject gameObject) where T : Object
    {
        T component = gameObject.GetComponent<T>();
        Debug.Assert(component);

        return component;
    }

    public static T GetComponentAssert<T>(this Transform transform) where T : Object
    {
        T component = transform.GetComponent<T>();
        Debug.Assert(component);

        return component;
    }
}