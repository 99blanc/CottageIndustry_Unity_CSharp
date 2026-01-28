using R3;
using System.Collections.Generic;

public partial class StatModel : ICharacterView, IEquipmentView
{
    ReadOnlyReactiveProperty<short> IBaseView.maxHealth => Get<short>(StatType.MAX_HEALTH);
    ReadOnlyReactiveProperty<short> IBaseView.damage => Get<short>(StatType.DAMAGE);
    ReadOnlyReactiveProperty<float> IBaseView.atkSpeed => Get<float>(StatType.ATK_SPEED);
    ReadOnlyReactiveProperty<float> IBaseView.moveSpeed => Get<float>(StatType.MOVE_SPEED);
    ReadOnlyReactiveProperty<short> IBaseView.dashCount => Get<short>(StatType.DASH_COUNT);
    ReadOnlyReactiveProperty<float> IBaseView.dashCooltime => Get<float>(StatType.DASH_COOLTIME);
    ReadOnlyReactiveProperty<float> IBaseView.dashDistance => Get<float>(StatType.DASH_DISTANCE);
    ReadOnlyReactiveProperty<short> IBaseView.jumpCount => Get<short>(StatType.JUMP_COUNT);
    ReadOnlyReactiveProperty<float> IBaseView.jumpForce => Get<float>(StatType.JUMP_FORCE);
    ReadOnlyReactiveProperty<float> IBaseView.gvReduction => Get<float>(StatType.GV_REDUCTION);
    ReadOnlyReactiveProperty<float> IBaseView.invulDuration => Get<float>(StatType.INVUL_DURATION);
    ReadOnlyReactiveProperty<WeaponCategory> IBaseView.weaponCategory => Get<WeaponCategory>(StatType.WEAPON_CATEGORY);
    ReadOnlyReactiveProperty<short> IHealthView.currentHealth => Get<short>(StatType.CURRENT_HEALTH);
    ReadOnlyReactiveProperty<short> IHealthView.maxHealth => Get<short>(StatType.MAX_HEALTH);
    ReadOnlyReactiveProperty<short> IHealthView.tempHealth => Get<short>(StatType.TEMP_HEALTH);
    ReadOnlyReactiveProperty<short> IActionView.dashCount => Get<short>(StatType.DASH_COUNT);
    ReadOnlyReactiveProperty<float> IActionView.dashCooltime => Get<float>(StatType.DASH_COOLTIME);
    ReadOnlyReactiveProperty<short> IActionView.jumpCount => Get<short>(StatType.JUMP_COUNT);
    ReadOnlyReactiveProperty<float> IMovementView.moveSpeed => Get<float>(StatType.MOVE_SPEED);
    ReadOnlyReactiveProperty<short> IMovementView.dashCount => Get<short>(StatType.DASH_COUNT);
    ReadOnlyReactiveProperty<float> IMovementView.dashCooltime => Get<float>(StatType.DASH_COOLTIME);
    ReadOnlyReactiveProperty<float> IMovementView.dashDistance => Get<float>(StatType.DASH_DISTANCE);
    ReadOnlyReactiveProperty<short> IMovementView.jumpCount => Get<short>(StatType.JUMP_COUNT);
    ReadOnlyReactiveProperty<float> IMovementView.jumpForce => Get<float>(StatType.JUMP_FORCE);
    ReadOnlyReactiveProperty<float> IMovementView.gvReduction => Get<float>(StatType.GV_REDUCTION);
    ReadOnlyReactiveProperty<float> IEquipmentView.atkRange => Get<float>(StatType.ATK_RANGE);
    ReadOnlyReactiveProperty<float> IEquipmentView.atkInterval => Get<float>(StatType.ATK_INTERVAL);
    ReadOnlyReactiveProperty<float> IEquipmentView.chargeMul => Get<float>(StatType.CHARGE_MULTIPLE);
    ReadOnlyReactiveProperty<short> IEquipmentView.pierceCount => Get<short>(StatType.PIERCE_COUNT);

    private readonly Dictionary<StatType, IStatView> stats = new();

    public ReactiveProperty<T> Get<T>(StatType type, T defaultValue = default) where T : struct
    {
        if (!stats.TryGetValue(type, out var stat))
        {
            stat = new StatView<T>(defaultValue);
            stats[type] = stat;
        }

        return ((StatView<T>)stat).property;
    }

    public void Set<T>(StatType type, T value) where T : struct => Get<T>(type).Value = value;
}
