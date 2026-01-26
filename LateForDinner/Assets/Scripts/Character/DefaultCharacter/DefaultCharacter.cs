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
        CharacterData data = Managers.Data.characters[CharacterID.DEFAULT];
        stats.maxHealth.Value = data.maxHealth;
        stats.damage.Value = data.damage;
        stats.dashCount.Value = data.dashCount;
        stats.jumpCount.Value = data.jumpCount;
        stats.gvReduction.Value = data.gvReduction;
        stats.atkSpeed.Value = data.atkSpeed;
        stats.moveSpeed.Value = data.moveSpeed;
        stats.jumpForce.Value = data.jumpForce;
        stats.dashDistance.Value = data.dashDistance;
        stats.dashCooltime.Value = data.dashCooltime;
        stats.invulDuration.Value = data.invulDuration;
        stats.weaponCategory = data.weaponCategory;
    }
}
