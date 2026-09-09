using R3;
using System;
using System.Collections.Generic;

internal static class PoolDisposableRegistry
{
    private static readonly Dictionary<IPoolable, CompositeDisposable> _bags = new Dictionary<IPoolable, CompositeDisposable>();

    public static void Register(IPoolable owner, IDisposable disposable)
    {
        if (owner == null || disposable == null)
            return;

        if (!_bags.TryGetValue(owner, out var bag))
        {
            bag = new CompositeDisposable();
            _bags[owner] = bag;
        }

        bag.Add(disposable);
    }

    public static void Clear(IPoolable owner)
    {
        if (owner == null)
            return;

        if (_bags.TryGetValue(owner, out var bag))
        {
            bag.Dispose();
            _bags.Remove(owner);
        }
    }
}
