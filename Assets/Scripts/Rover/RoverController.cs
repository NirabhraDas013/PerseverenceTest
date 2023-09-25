using System;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    [SerializeField] private WheelInfo[] wheelInfos;
    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] private float maxTorque = 300;


    [Header("Wheel Suspension")]
    [SerializeField] [Range(0, 20)] private float naturalFrequency = 10f;
    [SerializeField] [Range(0, 3)] private float dampingRatio = 0.8f;
    [SerializeField] [Range(-1, 1)] private float forceShift = 0.03f;
    [SerializeField] private bool setSuspensionDistance;

    [Header("Rover Arm")]
    [SerializeField] private Transform roverArmTarget;
    [SerializeField] private float maxArmSpeed = 10f;

    private float moveInputValue;
    private float turnInputValue;
    private Vector3 armMoveDirection;

    private bool isControlModeAutomatic = false;

    private void OnEnable()
    {
        GameEventsManager.instance.InputEvents.OnMoveInput += MoveInputReceived;
        GameEventsManager.instance.InputEvents.OnTurnInput += TurnInputReceived;
        GameEventsManager.instance.InputEvents.OnStopInput += StopInputReceived;
        GameEventsManager.instance.InputEvents.OnSwitchArmControlMode += SwitchControlMode;
        GameEventsManager.instance.InputEvents.OnArmMove += MoveRoverArm;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.InputEvents.OnMoveInput -= MoveInputReceived;
        GameEventsManager.instance.InputEvents.OnTurnInput -= TurnInputReceived;
        GameEventsManager.instance.InputEvents.OnStopInput -= StopInputReceived;
        GameEventsManager.instance.InputEvents.OnSwitchArmControlMode -= SwitchControlMode;
        GameEventsManager.instance.InputEvents.OnArmMove -= MoveRoverArm;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //Calculate the Wheel Joint forces
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            JointSpring spring = wheelCollider.suspensionSpring;

            spring.spring = Mathf.Pow(Mathf.Sqrt(wheelCollider.sprungMass) * naturalFrequency, 2);
            spring.damper = 2 * dampingRatio * Mathf.Sqrt(spring.spring * wheelCollider.sprungMass);

            wheelCollider.suspensionSpring = spring;

            Vector3 wheelRelativeBody = transform.InverseTransformPoint(wheelCollider.transform.position);
            float distance = GetComponent<Rigidbody>().centerOfMass.y - wheelRelativeBody.y + wheelCollider.radius;

            wheelCollider.forceAppPointDistance = distance - forceShift;

            //The following code makes sure that the spring Force at maximum drop is exactly zero
            if (spring.targetPosition > 0 && setSuspensionDistance)
            {
                wheelCollider.suspensionDistance = wheelCollider.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
            }
        }

        //Handle Rover Movement here
        float angle = maxAngle * turnInputValue;
        float torque = maxTorque * moveInputValue;

        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            int index = Array.IndexOf(wheelInfos, wheelInfo);
            WheelCollider wheel = wheelColliders[index];

            if (wheelInfo.isDrive)
            {
                wheel.motorTorque = torque;
            }

            if (wheelInfo.isTurn)
            {
                wheel.steerAngle = angle * wheelInfo.turnDirection;
                wheel.motorTorque = maxTorque * MathF.Abs(turnInputValue);
            }

            //Udate the Wheel objects
            Vector3 position;
            Quaternion rotation;
            wheel.GetWorldPose(out position, out rotation);

            Transform wheelTransform = wheelInfo.wheel.transform;
            wheelTransform.position = position;
            wheelTransform.rotation = rotation;
        }

        //Handle Rover Arm Movement here
        Vector3 moveDirection = maxArmSpeed * armMoveDirection;

        Vector3 currentTargetPos = roverArmTarget.localPosition;
        currentTargetPos += moveDirection * Time.deltaTime;
        roverArmTarget.localPosition = currentTargetPos;
    }

    private void MoveInputReceived(float moveInputValue)
    {
        this.moveInputValue = moveInputValue;
    }

    private void TurnInputReceived(float turnInputValue)
    {
        this.turnInputValue = turnInputValue;
    }

    private void StopInputReceived(bool isDriveStopping)
    {
        if (isDriveStopping)
        {
            moveInputValue = 0;
        }
        else
        {
            turnInputValue = 0;
        }
    }

    private void MoveRoverArm(Vector3 direction)
    {
        armMoveDirection = direction;
    }

    private void SwitchControlMode()
    {
        isControlModeAutomatic = !isControlModeAutomatic;
        //maybe do other things down the line
    }

    public bool GetControlMode()
    {
        return isControlModeAutomatic;
    }
}
