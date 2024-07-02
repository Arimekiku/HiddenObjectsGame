using UniRx;
using UnityEngine;

public interface ICounterModel
{
    public IntReactiveProperty Id { get; }
    public IntReactiveProperty Count { get; }
    public Vector3ReactiveProperty Position { get; }
    
    public int Delta { get; }

    public void Initialize(Vector3 position, int id, int delta = 1);
    public void AddDelta();
    public bool TryRemove(int value);
}