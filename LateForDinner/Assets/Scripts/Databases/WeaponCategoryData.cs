using MemoryPack;

namespace LateForDinner.Data
{
    [MemoryPackable]
    public partial class WeaponCategoryData
    {
        public string WeaponCategory { get; set; }
        public string LocalizationKey { get; set; }
        public int Bitmask { get; set; }
    }
}
