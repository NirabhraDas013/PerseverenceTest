using System;
using UnityEngine;

public class InputEvents
{
    public event Action<float> OnMoveInput;
    public void MoveInput(float moveDirection)
    {
        //Debug.Log($"Move Input Fired wih value {moveDirection}");
        OnMoveInput?.Invoke(moveDirection);
    }

    public event Action<float> OnTurnInput;
    public void TurnInput(float turnDirection)
    {
        //Debug.Log($"Turn Input Fired wih value {turnDirection}");
        OnTurnInput?.Invoke(turnDirection);
    }

    public event Action<bool> OnStopInput;
    public void StopInput(bool isMoveStop)
    {
        //Debug.Log($"Stop Input Fired wih value {isMoveStop}");
        OnStopInput?.Invoke(isMoveStop);
    }

    public event Action OnSwitchArmControlMode;
    public void SwitchArmControlMode()
    {
        OnSwitchArmControlMode?.Invoke();
    }

    public event Action<Vector3> OnArmMove;
    public void ArmMove(Vector3 direction)
    {
        Debug.Log("Arm Should Move");
        OnArmMove?.Invoke(direction);
    }

    public event Action<bool> OnSwitchPlayMode;
    public void SwitchPlayMode(bool generatePlane)
    {
        OnSwitchPlayMode?.Invoke(generatePlane);
    }
}
