using Cysharp.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtensions
{
    public static readonly List<Component> Caches = new(64);

    public static T FindChild<T>(this GameObject gameObject, string name = null, bool recursive = false) where T : Object
    {
        if (!gameObject)
            throw new InvalidOperationException();

        if (recursive)
        {
            lock (Caches)
            {
                Caches.Clear();
                gameObject.GetComponentsInChildren<T>(true, (List<T>)(object)Caches);

                for (int index = 0; index < Caches.Count; ++index)
                {
                    Object component = Caches[index];

                    if (string.IsNullOrEmpty(name) || ZString.Equals(name, Caches[index].name))
                        return component as T;
                }

                throw new InvalidOperationException();
            }
        }
        else
        {
            for (int index = 0; index < gameObject.transform.childCount; ++index)
            {
                Transform child = gameObject.transform.GetChild(index);

                if (!string.IsNullOrEmpty(name) && !ZString.Equals(name, child.name))
                    continue;

                if (child.TryGetComponent<T>(out T component))
                    return component;
            }
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

    public static T GetOrAddComponentAssert<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.TryGetComponent<T>(out T component))
            component = gameObject.AddComponent<T>();

        Debug.Assert(component);
        return component;
    }

    public static T GetComponentAssert<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        Debug.Assert(component);
        return component;
    }

    public static T GetComponentAssert<T>(this Transform transform) where T : Component
    {
        T component = transform.GetComponent<T>();
        Debug.Assert(component);
        return component;
    }
}