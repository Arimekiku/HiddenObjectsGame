using System;

public static class DisposableExtensions
{
    public static T AddTo<T>(this T disposable, DisposableEntity entity) where T : IDisposable
    {
        entity.AddDisposable(disposable);
        return disposable;
    }
}