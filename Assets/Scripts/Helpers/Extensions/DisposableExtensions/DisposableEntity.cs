using System;
using UniRx;

public class DisposableEntity : IDisposable
{
    private CompositeDisposable _disposables = new();

    public void Dispose()
    {
        _disposables.Dispose();
        _disposables = new CompositeDisposable();
    }

    public void AddDisposable(IDisposable item)
    {
        _disposables.Add(item);
    }
}