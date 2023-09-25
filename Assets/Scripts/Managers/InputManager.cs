using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Found More Than One Inputmanager in the Scene.");
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.UIEvents.OnButtonDown += OnButtonDown;
        GameEventsManager.instance.UIEvents.OnButtonUp += OnButtonUp;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.UIEvents.OnButtonDown -= OnButtonDown;
        GameEventsManager.instance.UIEvents.OnButtonUp -= OnButtonUp;
        
    }

    private void OnButtonDown(UIBUTTON buttonType)
    {
        switch (buttonType)
        {
            case UIBUTTON.FORWARD:
                //Fire Input Event Move with value 1
                GameEventsManager.instance.InputEvents.MoveInput(1);
                break;
            case UIBUTTON.REVERSE:
                //Fire Input Event Move with value -1
                GameEventsManager.instance.InputEvents.MoveInput(-1);
                break;
            case UIBUTTON.LEFT:
                //Fire Input Event Turn with value -1
                GameEventsManager.instance.InputEvents.TurnInput(-1);
                break;
            case UIBUTTON.RIGHT:
                //Fire Input Event Turn with value 1
                GameEventsManager.instance.InputEvents.TurnInput(1);
                break;
            case UIBUTTON.CONTROLMODE:
                //Switch ControlMode from Manual to Automatic and vice versa
                GameEventsManager.instance.InputEvents.SwitchArmControlMode();
                break;
            case UIBUTTON.ARMLEFT:
                //Fire Input Event ArmMove with Vector3.left
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.left);
                break;
            case UIBUTTON.ARMRIGHT:
                //Fire Input Event ArmMove with Vector3.right
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.right);
                break;
            case UIBUTTON.ARMUP:
                //Fire Input Event ArmMove with Vector3.up
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.up);
                break;
            case UIBUTTON.ARMDOWN:
                //Fire Input Event ArmMove with Vector3.down
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.down);
                break;
            case UIBUTTON.ARMFRONT:
                //Fire Input Event ArmMove with Vector3.forward
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.forward);
                break;
            case UIBUTTON.ARMBACK:
                //Fire Input Event ArmMove with Vector3.back
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.back);
                break;
            default:
                break;
        }
    }

    private void OnButtonUp(UIBUTTON buttonType)
    {
        switch (buttonType)
        {
            case UIBUTTON.FORWARD:
            case UIBUTTON.REVERSE:
                //Fire Stop Input event with value true
                GameEventsManager.instance.InputEvents.StopInput(true);
                break;
            case UIBUTTON.LEFT:
            case UIBUTTON.RIGHT:
                //Fire Stop Input event with value false
                GameEventsManager.instance.InputEvents.StopInput(false);
                break;
            case UIBUTTON.CONTROLMODE:
                //Do Nothing
                break;
            case UIBUTTON.ARMLEFT:
            case UIBUTTON.ARMRIGHT:
            case UIBUTTON.ARMUP:
            case UIBUTTON.ARMDOWN:
            case UIBUTTON.ARMFRONT:
            case UIBUTTON.ARMBACK:
                //Fire Input Event ArmMove with Vector3.zero
                GameEventsManager.instance.InputEvents.ArmMove(Vector3.zero);
                break;
            default:
                break;
        }
    }
}
