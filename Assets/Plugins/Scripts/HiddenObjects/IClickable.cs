using System;

public interface IClickable
{
    public event Action<IClickable> OnClickEvent; 
    
    public void Click();
}