using MemoryPack;

namespace LateForDinner.Data
{
    [MemoryPackable]
    public partial class ItemCategoryData
    {
        public string ItemCategory { get; set; }
        public string LocalizationKey { get; set; }
        public int Bitmask { get; set; }
    }
}
