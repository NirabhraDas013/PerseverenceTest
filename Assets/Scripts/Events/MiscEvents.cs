using System;

public class MiscEvents
{
    public event Action OnStartGame;
    public void StartGame()
    {
        OnStartGame?.Invoke();
    }
}
