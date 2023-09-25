using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestScript : MonoBehaviour
{
    Coroutine uiEventTestCoroutine;

    bool isPressed = false;

    private void OnEnable()
    {
        GameEventsManager.instance.UIEvents.OnButtonDown += UIEvents_OnButtonDown;
        GameEventsManager.instance.UIEvents.OnButtonUp += UIEvents_OnButtonUp;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.UIEvents.OnButtonDown -= UIEvents_OnButtonDown;
        GameEventsManager.instance.UIEvents.OnButtonUp -= UIEvents_OnButtonUp;
    }

    private void UIEvents_OnButtonDown(UIBUTTON uiButton)
    {
        isPressed = true;
        uiEventTestCoroutine = StartCoroutine(ButtonDownEvent(uiButton));
    }

    private void UIEvents_OnButtonUp(UIBUTTON uiButton)
    {
        isPressed = false;
        StopCoroutine(uiEventTestCoroutine);
        Debug.Log($"UI Button {uiButton} has been Released.");
    }

    IEnumerator ButtonDownEvent(UIBUTTON uiButton)
    {
        float timer = 0f;
        while (isPressed)
        {
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
            Debug.Log($"UI Button {uiButton} is being pressed for {timer} seconds."); 
        }
    }
}
