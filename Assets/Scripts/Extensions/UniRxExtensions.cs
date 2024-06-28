using System;
using TMPro;
using UniRx;

public static class UniRxExtensions
{
    public static IDisposable SubscribeToText<T>(this IObservable<T> observable, TextMeshProUGUI text)
    {
        return observable.SubscribeWithState(text, (x, t) => t.text = x.ToString());
    }
}