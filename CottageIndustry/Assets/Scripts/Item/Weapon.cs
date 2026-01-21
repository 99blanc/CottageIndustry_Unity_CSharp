using R3;
using UnityEngine;

public abstract class Weapon<T> : MonoBehaviour, IStat<T> where T : Component
{
    public StatModule module { get; set; } = new();
    public ReactiveProperty<float> atkRange { get; set; } = new();
    public ReactiveProperty<float> chargeMult { get; set; } = new();
    public ReactiveProperty<short> pierceCount { get; set; } = new();
    public ReactiveProperty<float> rapidInterval { get; set; } = new();
}