public class CharacterData
{
    public CharacterID id { get; set; }
    public short maxHealth { get; set; }
    public short tempHealth { get; set; }
    public short damage { get; set; }
    public short dashCount { get; set; }
    public short jumpCount { get; set; }
    public float gvReduction { get; set; }
    public float atkSpeed { get; set; }
    public float moveSpeed { get; set; }
    public float jumpForce { get; set; }
    public float dashDistance { get; set; }
    public float dashCooltime { get; set; }
    public float invulDuration { get; set; }
    public WeaponCategory weaponCategory { get; set; }
}
