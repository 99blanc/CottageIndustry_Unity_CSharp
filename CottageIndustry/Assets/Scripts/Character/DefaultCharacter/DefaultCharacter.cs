using UnityEngine;

public class DefaultCharacter : Character<Component>
{
    public override void Init()
    {
        base.Init();
        ApplyStat();
    }

    private void ApplyStat()
    {
        CharacterData data = Managers.Data.Character[CharacterID.DEFAULT];
        module.maxHealth.Value = data.maxHealth;
        module.damage.Value = data.damage;
        module.dashCount.Value = data.dashCount;
        module.jumpCount.Value = data.jumpCount;
        module.gvReduction.Value = data.gvReduction;
        module.atkSpeed.Value = data.atkSpeed;
        module.moveSpeed.Value = data.moveSpeed;
        module.jumpForce.Value = data.jumpForce;
        module.dashDistance.Value = data.dashDistance;
        module.dashCooltime.Value = data.dashCooltime;
        module.invulDuration.Value = data.invulDuration;
        module.weaponCategory = data.weaponCategory;
    }
}
