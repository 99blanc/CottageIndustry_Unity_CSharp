using R3;
using UnityEngine;

public class RangeWeapon<T> : Weapon<T> where T : Component
{
    public ReactiveProperty<float> projSpeed { get; set; } = new();
    public ReactiveProperty<short> projSize { get; set; } = new();
}
