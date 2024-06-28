using UniRx;

public interface IWalletService
{
    public IntReactiveProperty Coins { get; }
    public IntReactiveProperty Stars { get; }

    public void Earn(IntReactiveProperty property, int value);
    public bool TrySpend(IntReactiveProperty property, int value);
}