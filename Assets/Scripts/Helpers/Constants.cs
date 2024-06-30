using System.Collections.Generic;

public static class Constants
{
    public static readonly List<CollectableType> HiddenObjects = new List<CollectableType>
    {
        CollectableType.Hammer,
        CollectableType.Steerwheel,
        CollectableType.Salt,
        CollectableType.Joystick,
        CollectableType.Kettle,
    };

    public static readonly List<CollectableType> Currency = new List<CollectableType>
    {
        CollectableType.Coin,
        CollectableType.Star,
    };
}