using System;
using UniRx;

public class DisposableEntity : IDisposable
{
    private readonly CompositeDisposable _disposables = new();

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void AddDisposable(IDisposable item)
    {
        _disposables.Add(item);
    }
}