using System;
using UnityEngine;

public class UIEvents
{
    public event Action<UIBUTTON> OnButtonDown;
    public void ButtonDown(UIBUTTON uiButton)
    {
        Debug.Log("Button Down");
        OnButtonDown.Invoke(uiButton);
    }

    public Action<UIBUTTON> OnButtonUp;
    public void ButtonUp(UIBUTTON uiButton)
    {
        Debug.Log("Button Up");
        OnButtonUp.Invoke(uiButton);
    }
}
