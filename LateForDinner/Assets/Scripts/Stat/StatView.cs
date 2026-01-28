using R3;

public interface IStatView { }

public class StatView<T> : IStatView where T : struct
{
    public readonly ReactiveProperty<T> property;

    public StatView(T value) => property = new(value);
}
