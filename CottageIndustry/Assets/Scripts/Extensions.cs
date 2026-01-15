using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static void BindViewEvent(this UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component) => UserInterface.BindViewEvent(view, action, type, component);

    public static void BindModelEvent<T>(this IReadOnlyAsyncReactiveProperty<T> model, Action<T> action, Component component) => UserInterface.BindModelEvent(model, action, component);
}
