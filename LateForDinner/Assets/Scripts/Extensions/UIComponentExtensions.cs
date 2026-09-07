using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class UIComponentExtensions
{
    public static void SetVisual(this Image boxImage, Image toggleImage = null, Scrollbar scrollbar = null, bool isEnabled = true)
    {
        Color targetColor = isEnabled ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);

        if (boxImage != null)
            boxImage.color = targetColor;

        if (toggleImage != null)
            toggleImage.color = targetColor;

        if (scrollbar != null)
        {
            scrollbar.interactable = isEnabled;

            if (scrollbar.TryGetComponent<Image>(out var barImage))
                barImage.color = targetColor;

            if (scrollbar.targetGraphic is Graphic bgGraphic)
                bgGraphic.color = targetColor;

            if (scrollbar.handleRect != null && scrollbar.handleRect.TryGetComponent<Image>(out var handleImage))
                handleImage.color = targetColor;
        }
    }

    public static void SetActive(this Graphic component, bool isActive)
    {
        if (component != null && component.gameObject != null)
            component.gameObject.SetActive(isActive);
    }

    public static void SetActive(this TMP_Text component, bool isActive)
    {
        if (component != null && component.gameObject != null)
            component.gameObject.SetActive(isActive);
    }

    public static void SetActive(this Component component, bool isActive)
    {
        if (component != null && component.gameObject != null)
            component.gameObject.SetActive(isActive);
    }

    public static bool IsActive(this Component component)
        => component != null && component.gameObject != null && component.gameObject.activeInHierarchy;

    public static bool IsActive(this GameObject gameObject)
        => gameObject != null && gameObject.activeInHierarchy;
}
