using UnityEngine;

public abstract class Agent<T> : MonoBehaviour where T : Component
{
    public IAgentModule[] modules { get; protected set; }
    public StatModel registry { get; protected set; } = new();
    public IMovementView MoveView => registry;

    public virtual void Init(AgentData data)
    {
        ApplyRegistry(data);
        SetupModules(data);
    }

    protected abstract void ApplyRegistry(AgentData data);

    protected virtual void SetupModules(AgentData data)
    {
        modules = gameObject.GetComponentsAssert<IAgentModule>();

        foreach (var module in modules)
            module.Setup(registry, data);
    }
}
