using R3;

public partial class StatModule
{
    public ReactiveProperty<short> maxHealth { get; set; } = new(default);
    public ReactiveProperty<short> damage { get; set; } = new(default);
    public ReactiveProperty<short> dashCount { get; set; } = new(default);
    public ReactiveProperty<short> jumpCount { get; set; } = new(default);
    public ReactiveProperty<float> gvReduction { get; set; } = new(default);
    public ReactiveProperty<float> atkSpeed { get; set; } = new(default);
    public ReactiveProperty<float> moveSpeed { get; set; } = new(default);
    public ReactiveProperty<float> jumpForce { get; set; } = new(default);
    public ReactiveProperty<float> dashDistance { get; set; } = new(default);
    public ReactiveProperty<float> dashCooltime { get; set; } = new(default);
    public ReactiveProperty<float> invulDuration { get; set; } = new(default);
    public WeaponCategory weaponCategory { get; set; } = WeaponCategory.NULL;
}
