using R3;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Character<T> : MonoBehaviour, IStat<T> where T : Component
{
    public StatModule stats { get; set; } = new();
    public ReactiveProperty<short> currentHealth { get; set; } = new();
    public ReactiveProperty<short> tempHealth { get; set; } = new();

    public virtual void Init()
    {
        gameObject.GetOrAddComponent<CharacterControl>();
        gameObject.GetOrAddComponent<CharacterAnimation>();
    }

    public virtual void Heal(short amount)
    {
        currentHealth.Value += amount;

        if (currentHealth.Value > stats.maxHealth.Value)
            currentHealth.Value = stats.maxHealth.Value;
    }

    public virtual void TakeDamage(short damage)
    {
        currentHealth.Value -= damage;

        if (currentHealth.Value < 0)
            currentHealth.Value = 0;
    }
}
