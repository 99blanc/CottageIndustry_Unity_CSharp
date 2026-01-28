using R3;

public interface IBaseView
{
    ReadOnlyReactiveProperty<short> maxHealth { get; }
    ReadOnlyReactiveProperty<short> damage { get; }
    ReadOnlyReactiveProperty<float> atkSpeed { get; }
    ReadOnlyReactiveProperty<float> moveSpeed { get; }
    ReadOnlyReactiveProperty<short> dashCount { get; }
    ReadOnlyReactiveProperty<float> dashCooltime { get; }
    ReadOnlyReactiveProperty<float> dashDistance { get; }
    ReadOnlyReactiveProperty<short> jumpCount { get; }
    ReadOnlyReactiveProperty<float> jumpForce { get; }
    ReadOnlyReactiveProperty<float> gvReduction { get; }
    ReadOnlyReactiveProperty<float> invulDuration { get; }
    ReadOnlyReactiveProperty<WeaponCategory> weaponCategory { get; }
}

public interface IHealthView
{
    ReadOnlyReactiveProperty<short> currentHealth { get; }
    ReadOnlyReactiveProperty<short> maxHealth { get; }
    ReadOnlyReactiveProperty<short> tempHealth { get; }
}

public interface IActionView
{
    ReadOnlyReactiveProperty<short> dashCount { get; }
    ReadOnlyReactiveProperty<float> dashCooltime { get; }
    ReadOnlyReactiveProperty<short> jumpCount { get; }
}

public interface IMovementView
{
    ReadOnlyReactiveProperty<float> moveSpeed { get; }
    ReadOnlyReactiveProperty<short> dashCount { get; }
    ReadOnlyReactiveProperty<float> dashCooltime { get; }
    ReadOnlyReactiveProperty<float> dashDistance { get; }
    ReadOnlyReactiveProperty<short> jumpCount { get; }
    ReadOnlyReactiveProperty<float> jumpForce { get; }
    ReadOnlyReactiveProperty<float> gvReduction { get; }
}

public interface IAgentView : IMovementView { }

public interface ICharacterView : IAgentView, IBaseView, IHealthView, IActionView { }

public interface IEquipmentView : IBaseView, IMovementView
{
    ReadOnlyReactiveProperty<float> atkRange { get; }
    ReadOnlyReactiveProperty<float> atkInterval { get; }
    ReadOnlyReactiveProperty<float> chargeMul { get; }
    ReadOnlyReactiveProperty<short> pierceCount { get; }
}
