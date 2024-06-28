using UniRx;

public class WalletService : IWalletService
{
    public IntReactiveProperty Coins { get; }
    
    public WalletService()
    {
        Coins = new IntReactiveProperty(0);
    }

    public void Earn(int value)
    {
        Coins.Value += value;
    }

    public bool TrySpend(int value)
    {
        if (Coins.Value < value)
            return false;

        Coins.Value -= value;
        return true;
    }
}