using LateForDinner.Data;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using ZLinq;

public class UIQuestInventoryPopup : UIPopup, IDraggablePopup, IFocusablePopup
{
    private enum RectTransforms 
    { 
        EquipmentContent 
    }

    private enum Images
    {
        AttributeButtonImage, 
        TotalButtonImage, 
        EquipmentButtonImage,
        ConsumptionButtonImage, 
        EtcButtonImage, 
        SortButtonImage,
        ScrollUpArrowImage,
        ScrollDownArrowImage, 
        MealTimeImage
    }

    private enum Texts
    {
        AttributeTabText, 
        HealthTabText, 
        JumpForceTabText, 
        JumpCountTabText,
        DashDistanceTabText, 
        MoveSpeedTabText, 
        DamageTabText, 
        AttackSpeedTabText,
        GoldText, 
        DayText
    }

    private enum Buttons
    {
        AttributeButton, 
        TotalButton, 
        EquipmentButton, 
        ConsumptionButton,
        EtcButton, 
        SortButton, 
        ScrollUpButton, 
        ScrollDownButton
    }

    private enum ScrollRects 
    { 
        InventoryScrollRect 
    }

    private enum Panels 
    {
        AttributePanel 
    }

    private readonly ReactiveProperty<ButtonState> _attributeButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _totalButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _equipmentButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _consumptionButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _etcButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _sortButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _scrollUpButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly ReactiveProperty<ButtonState> _scrollDownButtonState = new ReactiveProperty<ButtonState>(ButtonState.Normal);
    private readonly List<UIInventorySlot> _createdSlots = new List<UIInventorySlot>();
    private readonly List<UIInventorySlot> _equipmentCreatedSlots = new List<UIInventorySlot>();
    private ItemType? _currentTabType = null;
    private bool _isAttributePanelOpen = true;
    public ItemType? CurrentTabType => _currentTabType;

    public override void OnInit()
    {
        base.OnInit();
        BindRectTransform(typeof(RectTransforms));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindScrollRect(typeof(ScrollRects));
        BindPanel(typeof(Panels));
        BindButtonStates();
        BindButtonActions();
        InitInventorySlots();
        InitEquipmentSlots();
        Managers.Inventory.OnInventoryChanged
        .Subscribe(_ => Refresh())
        .RegisterToPool(this);
    }

    private void BindButtonStates()
    {
        GetImage(Images.AttributeButtonImage).BindState(_attributeButtonState, Define.Atlas.Common, this);
        GetImage(Images.TotalButtonImage).BindState(_totalButtonState, Define.Atlas.Common, this);
        GetImage(Images.EquipmentButtonImage).BindState(_equipmentButtonState, Define.Atlas.Common, this);
        GetImage(Images.ConsumptionButtonImage).BindState(_consumptionButtonState, Define.Atlas.Common, this);
        GetImage(Images.EtcButtonImage).BindState(_etcButtonState, Define.Atlas.Common, this);
        GetImage(Images.SortButtonImage).BindState(_sortButtonState, Define.Atlas.Common, this);
        GetImage(Images.ScrollUpArrowImage).BindStateAsArrow(_scrollUpButtonState, Define.Atlas.Common, this);
        GetImage(Images.ScrollDownArrowImage).BindStateAsArrow(_scrollDownButtonState, Define.Atlas.Common, this);
    }

    private void BindButtonActions()
    {
        GetButton(Buttons.AttributeButton).BindViewAsButton(OnClickAttributeTab, ViewEvent.LeftClick, this, _attributeButtonState);
        GetButton(Buttons.TotalButton).BindViewAsButton(OnClickTotalTab, ViewEvent.LeftClick, this, _totalButtonState);
        GetButton(Buttons.EquipmentButton).BindViewAsButton(OnClickEquipmentTab, ViewEvent.LeftClick, this, _equipmentButtonState);
        GetButton(Buttons.ConsumptionButton).BindViewAsButton(OnClickConsumptionTab, ViewEvent.LeftClick, this, _consumptionButtonState);
        GetButton(Buttons.EtcButton).BindViewAsButton(OnClickEtcTab, ViewEvent.LeftClick, this, _etcButtonState);
        GetButton(Buttons.SortButton).BindViewAsButton(OnClickSortTab, ViewEvent.LeftClick, this, _sortButtonState);
        GetButton(Buttons.ScrollUpButton).BindViewAsButton(OnClickScrollUp, ViewEvent.LeftClick, this, _scrollUpButtonState);
        GetButton(Buttons.ScrollDownButton).BindViewAsButton(OnClickScrollDown, ViewEvent.LeftClick, this, _scrollDownButtonState);
    }

    private void InitInventorySlots()
    {
        var content = GetScrollRect(ScrollRects.InventoryScrollRect).content;

        for (int index = 0; index < Define.Amount.MaxInventorySlot; index++)
        {
            var (slot, _) = Managers.Pool.Pop<UIInventorySlot>(content);

            if (slot != null)
                _createdSlots.Add(slot);
        }
    }

    private void InitEquipmentSlots()
    {
        var equipmentContent = GetRectTransform(RectTransforms.EquipmentContent);

        for (int index = 0; index < Define.Amount.MaxEquipmentSlot; index++)
        {
            var (slot, _) = Managers.Pool.Pop<UIInventorySlot>(equipmentContent);

            if (slot != null)
                _equipmentCreatedSlots.Add(slot);
        }
    }

    public override void OnGet()
    {
        base.OnGet();
        Refresh();
    }

    public override void Refresh()
    {
        base.Refresh();
        RefreshInventory(_currentTabType);
        RefreshEquipmentSlots();

        if (_isAttributePanelOpen)
            RefreshPlayerInfo();
    }

    private void RefreshInventory(ItemType? type)
    {
        var slotDataList = Managers.Inventory.GetSlotsByType(type).ToList();
        int maxDisplayCount = type == null ? Define.Amount.MaxInventorySlot : Define.Amount.InventoryTabSize;

        for (int index = 0; index < _createdSlots.Count; index++)
        {
            if (index < maxDisplayCount)
            {
                _createdSlots[index].SetActive(true);

                if (index < slotDataList.Count)
                    _createdSlots[index].Setup(index, slotDataList[index], false);
            }
            else
                _createdSlots[index].SetActive(false);
        }
    }

    private void RefreshEquipmentSlots()
    {
        var equipmentDataList = Managers.Inventory.GetEquipmentSlots();

        for (int index = 0; index < _equipmentCreatedSlots.Count; index++)
        {
            InventorySlot targetData = equipmentDataList.FirstOrDefault(x => x.SlotIndex == index);
            _equipmentCreatedSlots[index].Setup(index, targetData, true);
        }
    }

    private void RefreshPlayerInfo()
    {
        var saveData = Managers.Save.CurrentData;

        if (saveData != null)
        {
            GetText(Texts.GoldText).text = saveData.Gold.ToString("N0");
            GetText(Texts.DayText).text = Managers.Localization.Get(LocalizationKey.Slot_Day_Format, saveData.Day);
            string spriteName = saveData.Meal.ToSpriteAsMealTime();
            GetImage(Images.MealTimeImage).sprite = Managers.Resource.GetSprite(Define.Atlas.Common, spriteName);
        }

        var player = Managers.Game.Player;

        if (player != null && player.Attributes != null)
        {
            GetText(Texts.HealthTabText).text = player.Attributes.Get<int>(AttributeType.Health).Value.ToString();
            GetText(Texts.MoveSpeedTabText).text = player.Attributes.Get<float>(AttributeType.MoveSpeed).Value.ToString("F1");
            GetText(Texts.DamageTabText).text = player.Attributes.Get<float>(AttributeType.Damage).Value.ToString("N0");
            GetText(Texts.AttackSpeedTabText).text = player.Attributes.Get<float>(AttributeType.AttackSpeed).Value.ToString("F2");
            GetText(Texts.JumpForceTabText).text = player.Attributes.Get<float>(AttributeType.JumpForce).Value.ToString("F1");
            GetText(Texts.JumpCountTabText).text = player.Attributes.Get<int>(AttributeType.JumpCount).Value.ToString();
            GetText(Texts.DashDistanceTabText).text = player.Attributes.Get<float>(AttributeType.DashDistance).Value.ToString("F1");
        }
    }

    private void OnClickAttributeTab(PointerEventData data)
    {
        _isAttributePanelOpen = !GetPanel(Panels.AttributePanel).IsActive();
        GetPanel(Panels.AttributePanel).SetActive(_isAttributePanelOpen);

        if (_isAttributePanelOpen) 
            RefreshPlayerInfo();
    }

    private void OnClickTotalTab(PointerEventData data) 
    { 
        _currentTabType = null; 
        RefreshInventory(_currentTabType); 
    }

    private void OnClickEquipmentTab(PointerEventData data) 
    { 
        _currentTabType = ItemType.Equipment; 
        RefreshInventory(_currentTabType); 
    }

    private void OnClickConsumptionTab(PointerEventData data) 
    {
        _currentTabType = ItemType.Consumption; 
        RefreshInventory(_currentTabType); 
    }

    private void OnClickEtcTab(PointerEventData data) 
    { 
        _currentTabType = ItemType.Etc; 
        RefreshInventory(_currentTabType); 
    }

    private void OnClickSortTab(PointerEventData data) 
    { 
        Managers.Inventory.SortInventory(_currentTabType); 
        Refresh(); 
    }

    private void OnClickScrollUp(PointerEventData data) 
        => GetScrollRect(ScrollRects.InventoryScrollRect).verticalNormalizedPosition = 1f;

    private void OnClickScrollDown(PointerEventData data) 
        => GetScrollRect(ScrollRects.InventoryScrollRect).verticalNormalizedPosition = 0f;
}
