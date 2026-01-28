using UnityEngine;

public class Player : Character<Component>
{
    public PlayerData pData { get; private set; }

    public void InitBySelection(PlayerID id)
    {
        if (!Managers.Data.players.TryGetValue(id, out var selectedData))
            return;

        Init(selectedData);
    }

    public override void Init(AgentData data)
    {
        gameObject.GetOrAddComponentAssert<PlayerControl>();
        base.Init(data);
    }

    protected override void ApplyRegistry(AgentData data)
    {
        pData = data as PlayerData;
        registry.Set(StatType.CURRENT_HEALTH, pData.maxHealth);
        registry.Set(StatType.MAX_HEALTH, pData.maxHealth);
        registry.Set(StatType.TEMP_HEALTH, pData.tempHealth);
        registry.Set(StatType.MOVE_SPEED, pData.moveSpeed);
        registry.Set(StatType.DAMAGE, pData.damage);
        registry.Set(StatType.ATK_SPEED, pData.atkSpeed);
        registry.Set(StatType.DASH_COUNT, pData.dashCount);
        registry.Set(StatType.DASH_COOLTIME, pData.dashCooltime);
        registry.Set(StatType.DASH_DISTANCE, pData.dashDistance);
        registry.Set(StatType.JUMP_COUNT, pData.jumpCount);
        registry.Set(StatType.JUMP_FORCE, pData.jumpForce);
        registry.Set(StatType.GV_REDUCTION, pData.gvReduction);
        registry.Set(StatType.INVUL_DURATION, pData.invulDuration);
        registry.Set(StatType.WEAPON_CATEGORY, pData.weaponCategory);
    }
}
