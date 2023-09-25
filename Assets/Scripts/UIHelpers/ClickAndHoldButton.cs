using UnityEngine;
using UnityEngine.UIElements;

public class ClickAndHoldButton : PointerManipulator
{
    private bool Enabled { get; set; }

    private VisualElement Root { get; }

    private UIBUTTON UIButton { get; }

    public ClickAndHoldButton(VisualElement target, UIBUTTON uiButton)
    {
        this.target = target;
        Root = target.parent;
        UIButton = uiButton;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        //Register the PointerDown and PointerUp callbacks
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        //Unregister the PointerDown and PointerUp callbacks
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    private void PointerDownHandler(PointerDownEvent @event)
    {
        Debug.Log($"pointer Pressed on {UIButton} button with pointerID {@event.pointerId}");
        target.CapturePointer(@event.pointerId);
        GameEventsManager.instance.UIEvents.ButtonDown(UIButton);
        Enabled = true;
    }

    private void PointerUpHandler(PointerUpEvent @event)
    {
        if (Enabled && target.HasPointerCapture(@event.pointerId))
        {
            Debug.Log($"pointer Released on {UIButton} button with pointerID {@event.pointerId}");
            target.ReleasePointer(@event.pointerId);
            GameEventsManager.instance.UIEvents.ButtonUp(UIButton);
            Enabled = false;
        }
    }
}
