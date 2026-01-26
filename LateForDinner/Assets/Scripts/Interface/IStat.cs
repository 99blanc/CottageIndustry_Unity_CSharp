using UnityEngine;

public interface IStat<T> where T : Component
{
    public StatModule stats { get; set; }
}
