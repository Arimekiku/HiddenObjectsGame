using UniRx;

public class WalletService : IWalletService
{
    public IntReactiveProperty Coins { get; }
    public IntReactiveProperty Stars { get; }
    
    public WalletService()
    {
        Coins = new IntReactiveProperty(0);

        Stars = new IntReactiveProperty(0);
    }
    
    public void Earn(IntReactiveProperty property, int value)
    {
        property.Value += value;
    }

    public bool TrySpend(IntReactiveProperty property, int value)
    {
        if (property.Value < value)
            return false;

        property.Value -= value;
        return true;
    }
}