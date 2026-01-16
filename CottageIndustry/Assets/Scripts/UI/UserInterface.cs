using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public enum ViewEvent
{
    ENTER,
    EXIT,
    LEFT_CLICK,
    RIGHT_CLICK,
}

public abstract class UserInterface : MonoBehaviour
{
    private Dictionary<Type, Object[]> objects = new();

    public virtual void Init() => objects.Clear();

    private void Start() => Init();

    protected void Bind<T>(Type type) where T : Object
    {
        Array values = Enum.GetValues(type);
        Object[] newObjects = new Object[values.Length];
        objects.Add(typeof(T), newObjects);

        for (int index = 0; index < values.Length; ++index)
            newObjects[index] = typeof(T) == typeof(GameObject) ? gameObject.FindChild(ZString.Concat(values.GetValue(index)), true) : gameObject.FindChild<T>(ZString.Concat(values.GetValue(index)), true);
    }

    protected T Get<T>(int index) where T : Object
    {
        if (objects.TryGetValue(typeof(T), out Object[] newObjects))
        {
            if (index < 0 || index >= newObjects.Length)
                throw new IndexOutOfRangeException();

            return newObjects[index] as T;
        }

        throw new InvalidOperationException();
    }

    protected void BindObject(Type type) => Bind<GameObject>(type);
    protected void BindImage(Type type) => Bind<Image>(type);
    protected void BindText(Type type) => Bind<TMP_Text>(type);
    protected void BindButton(Type type) => Bind<Button>(type);

    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected Image GetImage(int index) => Get<Image>(index);
    protected TMP_Text GetText(int index) => Get<TMP_Text>(index);
    protected Button GetButton(int index) => Get<Button>(index);

    public static void BindViewEvent(UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
    {
        CancellationToken token = component.GetCancellationTokenOnDestroy();
        IUniTaskAsyncEnumerable<PointerEventData> enumerable = type switch
        {
            ViewEvent.ENTER => view.gameObject.GetAsyncPointerEnterTrigger(),
            ViewEvent.EXIT => view.gameObject.GetAsyncPointerExitTrigger(),
            ViewEvent.LEFT_CLICK => view.gameObject.GetAsyncPointerClickTrigger()
                .Where(data => data.button == PointerEventData.InputButton.Left),
            ViewEvent.RIGHT_CLICK => view.gameObject.GetAsyncPointerDownTrigger()
                .Where(data => data.button == PointerEventData.InputButton.Right),
            _ => throw new ArgumentOutOfRangeException()
        };
        enumerable.Subscribe(action, token);
    }

    public static void BindModelEvent<T>(IReadOnlyAsyncReactiveProperty<T> model, Action<T> action, Component component)
    {
        model.ForEachAsync(action, component.GetCancellationTokenOnDestroy()).Forget();
    }
}
