using LateForDinner.Data;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using ZLinq;

public class InventoryManager
{
    private List<InventorySlot> _slots = new List<InventorySlot>(Define.Amount.MaxInventorySlot);
    private List<InventorySlot> _equipmentSlots = new List<InventorySlot>(Define.Amount.MaxEquipmentSlot);
    private List<InventorySlot> _quickSlots = new List<InventorySlot>(Define.Amount.MaxQuickSlot);
    public Subject<Unit> OnInventoryChanged = new Subject<Unit>();

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
    }

    public bool AddItem(int itemID, int quantity)
    {
        if (!TryGetValidItemData(itemID, out var itemData, out var itemType))
            return false;

        int tabSize = Define.Amount.InventoryTabSize;
        int startIndex = GetTabStartIndex(itemType);

        if (!HasEnoughSpaceInTab(itemID, itemData.MaxStack, startIndex, tabSize, quantity))
            return false;

        FillExistingItemSlotsInTab(itemID, itemData.MaxStack, startIndex, tabSize, ref quantity);
        FillEmptySlotsInTab(itemID, itemData.MaxStack, startIndex, tabSize, ref quantity);
        OnInventoryChanged.OnNext(Unit.Default);
        return true;
    }

    public bool RemoveItem(int itemID, int quantity)
    {
        int remainingToRemove = quantity;

        for (int index = 0; index < _slots.Count; index++)
        {
            if (remainingToRemove <= 0)
                break;

            var slot = _slots[index];

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
            OnInventoryChanged.OnNext(Unit.Default);

        return success;
    }

    public bool HandleItemMove(SlotArea sourceArea, InventorySlot sourceSlot, SlotArea targetArea, InventorySlot targetSlot)
    {
        if (sourceSlot == null || targetSlot == null || sourceSlot == targetSlot)
            return false;

        bool success = (sourceArea, targetArea) switch
        {
            (SlotArea.Inventory, SlotArea.Inventory) => MoveItem(sourceSlot, targetSlot),
            (SlotArea.Inventory, SlotArea.Equipment) => SwapSlotsReturn(sourceSlot, targetSlot),
            (SlotArea.Equipment, SlotArea.Inventory) => SwapSlotsReturn(sourceSlot, targetSlot),
            (SlotArea.Equipment, SlotArea.Equipment) => SwapSlotsReturn(sourceSlot, targetSlot),
            _ => false
        };

        if (success)
            OnInventoryChanged.OnNext(Unit.Default);

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

        OnInventoryChanged.OnNext(Unit.Default);
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

    private bool HasEnoughSpaceInTab(int itemID, int maxStack, int startIndex, int tabSize, int quantity)
    {
        int required = quantity;

        for (int index = 0; index < tabSize && required > 0; index++)
        {
            var slot = _slots[startIndex + index];
            if (slot.ItemID == itemID && slot.Quantity < maxStack)
                required -= (maxStack - slot.Quantity);
        }

        for (int index = 0; index < tabSize && required > 0; index++)
        {
            var slot = _slots[startIndex + index];
            if (slot.ItemID == 0)
                required -= maxStack;
        }

        return required <= 0;
    }

    private void FillExistingItemSlotsInTab(int itemID, int maxStack, int startIndex, int tabSize, ref int remaining)
    {
        for (int index = 0; index < tabSize && remaining > 0; index++)
        {
            var slot = _slots[startIndex + index];
            if (slot.ItemID != itemID || slot.Quantity >= maxStack)
                continue;

            int add = Math.Min(remaining, maxStack - slot.Quantity);
            slot.Quantity += add;
            remaining -= add;
        }
    }

    private void FillEmptySlotsInTab(int itemID, int maxStack, int startIndex, int tabSize, ref int remaining)
    {
        for (int index = 0; index < tabSize && remaining > 0; index++)
        {
            var slot = _slots[startIndex + index];
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

    private bool SwapSlotsReturn(InventorySlot a, InventorySlot b)
    {
        SwapSlots(a, b);
        return true;
    }

    private void SwapSlots(InventorySlot a, InventorySlot b)
    {
        (a.ItemID, b.ItemID) = (b.ItemID, a.ItemID);
        (a.Quantity, b.Quantity) = (b.Quantity, a.Quantity);
    }

    public List<InventorySlot> ExportSaveData() =>
        _slots.Select(slot => new InventorySlot { GlobalIndex = slot.GlobalIndex, SlotIndex = slot.SlotIndex, ItemID = slot.ItemID, Quantity = slot.Quantity }).ToList();

    public List<InventorySlot> ExportQuickSlotSaveData() =>
        _quickSlots.Select(slot => new InventorySlot { GlobalIndex = slot.GlobalIndex, SlotIndex = slot.SlotIndex, ItemID = slot.ItemID, Quantity = slot.Quantity }).ToList();

    public List<InventorySlot> ExportEquipmentSlotSaveData() =>
        _equipmentSlots.Select(slot => new InventorySlot { GlobalIndex = slot.GlobalIndex, SlotIndex = slot.SlotIndex, ItemID = slot.ItemID, Quantity = slot.Quantity }).ToList();

    public void ClearInventory()
    {
        ClearAllSlots(_slots);
        ClearAllSlots(_equipmentSlots);
        ClearAllSlots(_quickSlots);
        OnInventoryChanged.OnNext(Unit.Default);
    }

    private void ClearAllSlots(List<InventorySlot> slots)
    {
        foreach (var slot in slots)
            ClearSlot(slot);
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
}
