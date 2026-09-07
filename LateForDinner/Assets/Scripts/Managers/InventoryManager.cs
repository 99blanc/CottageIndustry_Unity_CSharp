using LateForDinner.Data;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using ZLinq;

public class InventoryManager
{
    public Observable<Unit> OnInventoryChanged => _onInventoryChanged;
    private readonly Subject<Unit> _onInventoryChanged = new Subject<Unit>();
    private List<InventorySlot> _slots = new List<InventorySlot>(Define.Amount.MaxInventorySlot);
    private List<InventorySlot> _equipmentSlots = new List<InventorySlot>(Define.Amount.MaxEquipmentSlot);
    private List<InventorySlot> _quickSlots = new List<InventorySlot>(Define.Amount.MaxQuickSlot);

    public void InitInventory(List<InventorySlot> savedSlots, List<InventorySlot> savedQuickSlots, List<InventorySlot> savedEquipmentSlots = null)
    {
        _slots = savedSlots ?? new List<InventorySlot>();
        EnsureSlotCapacity(_slots, Define.Amount.MaxInventorySlot);
        _equipmentSlots = savedEquipmentSlots ?? new List<InventorySlot>();
        EnsureSlotCapacity(_equipmentSlots, Define.Amount.MaxEquipmentSlot);
        _quickSlots = savedQuickSlots ?? new List<InventorySlot>();
        EnsureSlotCapacity(_quickSlots, Define.Amount.MaxQuickSlot);
    }

    private void EnsureSlotCapacity(List<InventorySlot> slots, int maxCapacity)
    {
        while (slots.Count < maxCapacity)
        {
            int index = slots.Count;
            slots.Add(new InventorySlot
            {
                GlobalIndex = index,
                SlotIndex = index,
                ItemID = 0,
                Quantity = 0
            });
        }

        for (int i = 0; i < slots.Count; i++)
            slots[i].GlobalIndex = i;
    }

    public bool AddItem(int itemID, int quantity)
    {
        if (!TryGetValidItemData(itemID, out var itemData, out var itemType))
            return false;

        if (!HasEnoughSpaceInAll(_slots, itemID, itemData.MaxStack, quantity))
            return false;

        FillExistingItemSlots(_slots, itemID, itemData.MaxStack, ref quantity);
        FillEmptySlots(_slots, itemID, itemData.MaxStack, ref quantity);
        _onInventoryChanged.OnNext(Unit.Default);
        return true;
    }

    public bool RemoveItem(int itemID, int quantity)
    {
        int remainingToRemove = quantity;

        foreach (var slot in _slots)
        {
            if (remainingToRemove <= 0) 
                break;

            if (slot.ItemID != itemID || slot.Quantity <= 0) 
                continue;

            int removeAmount = Math.Min(remainingToRemove, slot.Quantity);
            slot.Quantity -= removeAmount;
            remainingToRemove -= removeAmount;

            if (slot.Quantity <= 0) 
                ClearSlot(slot);
        }

        bool success = remainingToRemove < quantity;

        if (success)
            _onInventoryChanged.OnNext(Unit.Default);

        return success;
    }

    public bool HandleItemMoveByGlobalIndex(SlotArea sourceArea, int sourceGlobalIndex, SlotArea targetArea, int targetGlobalIndex)
    {
        if (sourceArea != SlotArea.Inventory || targetArea != SlotArea.Inventory)
            return false;

        if (sourceGlobalIndex < 0 || sourceGlobalIndex >= _slots.Count || targetGlobalIndex < 0 || targetGlobalIndex >= _slots.Count)
            return false;

        var sourceSlot = _slots[sourceGlobalIndex];
        var targetSlot = _slots[targetGlobalIndex];

        if (sourceSlot == targetSlot) 
            return false;

        bool success = MoveItem(sourceSlot, targetSlot);

        if (success)
            _onInventoryChanged.OnNext(Unit.Default);

        return success;
    }

    private bool MoveItem(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot.ItemID == 0) 
            return false;

        if (toSlot.ItemID == 0)
        {
            toSlot.ItemID = fromSlot.ItemID;
            toSlot.Quantity = fromSlot.Quantity;
            ClearSlot(fromSlot);
            return true;
        }

        if (toSlot.ItemID == fromSlot.ItemID)
        {
            var itemData = Managers.Data.Items[toSlot.ItemID];
            int availableSpace = itemData.MaxStack - toSlot.Quantity;

            if (availableSpace > 0)
            {
                int transferAmount = Math.Min(fromSlot.Quantity, availableSpace);
                toSlot.Quantity += transferAmount;
                fromSlot.Quantity -= transferAmount;

                if (fromSlot.Quantity <= 0) 
                    ClearSlot(fromSlot);

                return true;
            }
        }

        SwapSlots(fromSlot, toSlot);
        return true;
    }

    public void SortInventory(ItemType? currentTabType)
    {
        if (!currentTabType.HasValue)
        {
            var validItems = _slots.Where(slot => slot.ItemID != 0 && slot.Quantity > 0).OrderBy(slot => slot.ItemID).ToList();

            for (int index = 0; index < _slots.Count; index++)
            {
                if (index < validItems.Count)
                {
                    _slots[index].ItemID = validItems[index].ItemID;
                    _slots[index].Quantity = validItems[index].Quantity;
                }
                else
                    ClearSlot(_slots[index]);
            }
        }
        else
        {
            int tabSize = Define.Amount.InventoryTabSize;
            int startIndex = GetTabStartIndex(currentTabType.Value);
            var tabRange = _slots.Skip(startIndex).Take(tabSize).ToList();
            var validItems = tabRange.Where(s => s.ItemID != 0 && s.Quantity > 0).OrderBy(s => s.ItemID).ToList();

            for (int index = 0; index < tabSize; index++)
            {
                var slot = tabRange[index];

                if (index < validItems.Count)
                {
                    slot.ItemID = validItems[index].ItemID;
                    slot.Quantity = validItems[index].Quantity;
                }
                else
                    ClearSlot(slot);
            }
        }

        _onInventoryChanged.OnNext(Unit.Default);
    }

    public IEnumerable<InventorySlot> GetSlotsByType(ItemType? currentTabType)
    {
        if (!currentTabType.HasValue)
            return _slots;

        int tabSize = Define.Amount.InventoryTabSize;
        int startIndex = GetTabStartIndex(currentTabType.Value);
        return _slots.Skip(startIndex).Take(tabSize);
    }

    public IReadOnlyList<InventorySlot> GetEquipmentSlots() 
        => _equipmentSlots;

    public IReadOnlyList<InventorySlot> GetQuickSlots() 
        => _quickSlots;

    private bool TryGetValidItemData(int itemID, out ItemData itemData, out ItemType itemType)
    {
        itemData = null;
        itemType = ItemType.Etc;

        if (!Managers.Data.Items.ContainsKey(itemID))
            return false;

        itemData = Managers.Data.Items[itemID];
        Enum.TryParse(itemData.ItemType, true, out itemType);
        return true;
    }

    private bool HasEnoughSpaceInAll(List<InventorySlot> slots, int itemID, int maxStack, int quantity)
    {
        int required = quantity;
        foreach (var slot in slots)
        {
            if (required <= 0) 
                break;

            if (slot.ItemID == itemID && slot.Quantity < maxStack)
                required -= (maxStack - slot.Quantity);
            else if (slot.ItemID == 0)
                required -= maxStack;
        }

        return required <= 0;
    }

    private void FillExistingItemSlots(List<InventorySlot> slots, int itemID, int maxStack, ref int remaining)
    {
        foreach (var slot in slots)
        {
            if (remaining <= 0) 
                break;

            if (slot.ItemID != itemID || slot.Quantity >= maxStack) 
                continue;

            int add = Math.Min(remaining, maxStack - slot.Quantity);
            slot.Quantity += add;
            remaining -= add;
        }
    }

    private void FillEmptySlots(List<InventorySlot> slots, int itemID, int maxStack, ref int remaining)
    {
        foreach (var slot in slots)
        {
            if (remaining <= 0) 
                break;

            if (slot.ItemID != 0) 
                continue;

            int add = Math.Min(remaining, maxStack);
            slot.ItemID = itemID;
            slot.Quantity = add;
            remaining -= add;
        }
    }

    private void ClearSlot(InventorySlot slot)
    {
        slot.ItemID = 0;
        slot.Quantity = 0;
    }

    private void SwapSlots(InventorySlot a, InventorySlot b)
    {
        (a.ItemID, b.ItemID) = (b.ItemID, a.ItemID);
        (a.Quantity, b.Quantity) = (b.Quantity, a.Quantity);
    }

    public int GetTabStartIndex(ItemType? type)
    {
        int tabSize = Define.Amount.InventoryTabSize;
        return type switch
        {
            ItemType.Equipment => tabSize * 0,
            ItemType.Consumption => tabSize * 1,
            ItemType.Etc => tabSize * 2,
            _ => 0
        };
    }

    public void ClearInventory()
    {
        foreach (var slot in _slots)
            ClearSlot(slot);

        foreach (var slot in _equipmentSlots)
            ClearSlot(slot);

        foreach (var slot in _quickSlots)
            ClearSlot(slot);

        _onInventoryChanged.OnNext(Unit.Default);
    }

    public List<InventorySlot> ExportSaveData()
    {
        return _slots
        .Select(slot => slot == null ? null : new InventorySlot
        {
            GlobalIndex = slot.GlobalIndex,
            SlotIndex = slot.SlotIndex,
            ItemID = slot.ItemID,
            Quantity = slot.Quantity
        }).ToList();
    }

    public List<InventorySlot> ExportQuickSlotSaveData()
    {
        return _quickSlots
        .Select(slot => slot == null ? null : new InventorySlot
        {
            GlobalIndex = slot.GlobalIndex,
            SlotIndex = slot.SlotIndex,
            ItemID = slot.ItemID,
            Quantity = slot.Quantity
        }).ToList();
    }

    public List<InventorySlot> ExportEquipmentSlotSaveData()
    {
        return _equipmentSlots
        .Select(slot => slot == null ? null : new InventorySlot
        {
            GlobalIndex = slot.GlobalIndex,
            SlotIndex = slot.SlotIndex,
            ItemID = slot.ItemID,
            Quantity = slot.Quantity
        }).ToList();
    }
}
