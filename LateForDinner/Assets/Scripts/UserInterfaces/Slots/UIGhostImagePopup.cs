using UnityEngine;

public class UIGhostImagePopup : UIPopup, IDraggablePopup, IFocusablePopup
{
    private enum Images
    {
        SlotItemImage
    }

    public override void OnInit()
    {
        base.OnInit();
        BindImage(typeof(Images));
        this.GetComponentAssert<CanvasGroup>().blocksRaycasts = false;
    }

    public override void OnRelease()
    {
        base.OnRelease();
        GetImage(Images.SlotItemImage).sprite = null;
    }

    public void SetItemImage(Sprite sprite)
        => GetImage(Images.SlotItemImage).sprite = sprite;
}
