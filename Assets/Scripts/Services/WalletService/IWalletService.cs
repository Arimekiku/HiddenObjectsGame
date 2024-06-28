using UniRx;

public interface IWalletService
{
    public IntReactiveProperty Coins { get; }

    public void Earn(int value);
    public bool TrySpend(int value);
}