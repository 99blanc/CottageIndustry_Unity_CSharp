using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtensions
{
    public static readonly StringBuilder String = new();

    public static bool IsNullOrEmpty(this StringBuilder name) => name == null || name.Length == 0;

    public static bool Equals(this StringBuilder name, string span)
    {
        if (name is null || span is null)
            return name is null && span is null;

        if (name.Length != span.Length)
            return false;

        for (int i = 0; i < name.Length; ++i)
        {
            if (name[i] != span[i])
                return false;
        }

        return true;
    }

    public static T FindChild<T>(this GameObject go, StringBuilder name = null, bool recursive = false) where T : Object
    {
        if (!go)
            throw new InvalidOperationException();

        if (recursive)
            return go.GetComponentsInChildren<T>(true).FirstOrDefault(child => name.IsNullOrEmpty() || name.Equals(child.name)) ?? throw new InvalidOperationException();

        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);

            if ((name.IsNullOrEmpty() || name.Equals(child.name)) && child.TryGetComponent<T>(out T comp))
                return comp;
        }

        throw new InvalidOperationException();
    }

    public static GameObject FindChild(this GameObject gameObject, StringBuilder name = null, bool recursive = false) => FindChild<Transform>(gameObject, name, recursive).gameObject;

    public static Transform FindAssert(this Transform transform, StringBuilder name)
    {
        Transform newTransform = transform.Find(name.ToString());
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