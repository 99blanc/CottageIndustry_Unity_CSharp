using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<UIPopup> popupStack;

    private static readonly Vector3 DEFAULT_SCALE = Vector3.one;
    private int currentCanvasOrder = -20;
    private GameObject container;

    public void Init()
    {
        popupStack = new Stack<UIPopup>();
        container = new GameObject(nameof(container));
    }

    public void SetCanvas(GameObject gameObject, bool sort = true)
    {
        Canvas canvas = gameObject.GetComponentAssert<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = currentCanvasOrder;
            currentCanvasOrder += 1;

            return;
        }

        canvas.sortingOrder = 0;
    }

    public T OpenPopup<T>(Transform parent = null) where T : UIPopup
    {
        T popup = SetupUI<T>(parent);
        popupStack.Push(popup);

        return popup;
    }

    public T OpenSubItem<T>(Transform parent = null) where T : UISubItem => SetupUI<T>(parent);

    private T SetupUI<T>(Transform parent = null) where T : UserInterface
    {
        GameObject prefab = Managers.Resource.LoadPrefab(typeof(T).Name);
        GameObject gameObject = Managers.Resource.Instantiate(prefab);
        gameObject.transform.localScale = DEFAULT_SCALE;
        gameObject.transform.localPosition = prefab.transform.position;
        gameObject.transform.SetParent(container.transform);

        if (parent)
            gameObject.transform.SetParent(parent);

        return gameObject.GetComponentAssert<T>();
    }

    public void ClosePopup(UIPopup popup)
    {
        if (popupStack.Count == 0 || popupStack.Peek() != popup)
            return;

        ClosePopup();
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0)
            return;

        UIPopup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        currentCanvasOrder -= 1;
    }

    public void CloseAllPopup()
    {
        while (popupStack.Count > 0)
            ClosePopup();
    }
}
