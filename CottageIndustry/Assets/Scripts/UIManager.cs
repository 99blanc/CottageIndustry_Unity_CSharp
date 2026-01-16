using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<UIPopup> popupStack = new();

    private static readonly Vector3 DEFAULT_SCALE = Vector3.one;
    private int currentCanvasOrder = -20;
    private GameObject container;

    public void Init() => container = new(nameof(container));

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

    public async UniTask<T> OpenPopup<T>(Transform parent = null) where T : UIPopup
    {
        T popup = await SetupUI<T>(parent);
        popupStack.Push(popup);

        return popup;
    }

    public async UniTask<T> OpenSubItem<T>(Transform parent = null) where T : UISubItem => await SetupUI<T>(parent);

    private async UniTask<T> SetupUI<T>(Transform parent = null) where T : UserInterface
    {
        GameObject prefab = await Managers.Resource.LoadPrefab(typeof(T).Name);
        GameObject gameObject = Managers.Resource.Instantiate(prefab);
        Transform transform = gameObject.transform;
        transform.localScale = DEFAULT_SCALE;
        transform.localPosition = prefab.transform.position;
        Transform targetParent = parent != null ? parent : container.transform;
        transform.SetParent(targetParent);

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
