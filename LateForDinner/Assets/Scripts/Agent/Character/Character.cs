using UnityEngine;

public abstract class Character<T> : Agent<T> where T : Component
{
    public ICharacterView characterView => registry;
    public IHealthView healthView => registry;

    public override void Init(AgentData data) => base.Init(data);

    protected override void ApplyRegistry(AgentData data) { }

    protected override void SetupModules(AgentData data) => base.SetupModules(data);

    public virtual void RestoreHealth(short amount)
    {
        var cur = registry.Get<short>(StatType.CURRENT_HEALTH);
        var max = registry.Get<short>(StatType.MAX_HEALTH).Value;
        cur.Value = (short)Mathf.Min(cur.Value + amount, max);
    }

    public virtual void TakeDamage(short damage)
    {
        var cur = registry.Get<short>(StatType.CURRENT_HEALTH);
        cur.Value = (short)Mathf.Max(cur.Value - damage, 0);
    }
}
