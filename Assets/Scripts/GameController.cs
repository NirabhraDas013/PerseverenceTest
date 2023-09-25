using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class GameController : MonoBehaviour
{
    public static GameController instance;


    [SerializeField]private GameObject rover;

    [SerializeField]private RoverController roverController;
    [SerializeField]private Camera mainARCamera;
    [SerializeField]private ARRaycastManager arRaycastManager;
    [SerializeField]private ARPlaneManager arPlaneManager;

    [SerializeField] private bool isReady = true;
    private bool isRoverActive = false;
    private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Found More Than One GameController in the Scene.");
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaitAndTurnOnLayers());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.InputEvents.OnSwitchPlayMode += SwitchPlayMode;

        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;

        //PlayerInput.SwitchCurrentControlScheme(InputSystem.devices.First(d => d == Touchscreen.current));
    }

    private void OnDisable()
    {
        GameEventsManager.instance.InputEvents.OnSwitchPlayMode -= SwitchPlayMode;

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(Finger finger)
    {
        Debug.Log($"Tapped at {finger.currentTouch.screenPosition} with {finger.index} finger");
        if (!isRoverActive)
        {
            if (finger.index != 0)
            {
                Debug.Log("Only single touch is allowed");
                return;
            }

            if (arRaycastManager.Raycast(finger.currentTouch.screenPosition, arRaycastHits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in arRaycastHits)
                {
                    Pose pose = hit.pose;

                    if (arPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        rover.transform.position = pose.position;
                        rover.SetActive(true);
                        isRoverActive = true;

                        //Move to Play Mode and turn off planemanager
                        StartGame();
                        return;
                    }
                }
            }
        }
    }

    private void StartGame()
    {
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            if (plane.alignment != PlaneAlignment.HorizontalUp)
            {
                plane.gameObject.SetActive(false);
            }
        }

        arPlaneManager.enabled = false;

        //Fire Event
        GameEventsManager.instance.MiscEvents.StartGame();
    }

    public bool GetRoverControlMode()
    {
        return roverController.GetControlMode();
    }

    private void SwitchPlayMode(bool generatePlane)
    {
        arPlaneManager.enabled = generatePlane;

        if (generatePlane)
        {
            //rover.SetActive(false);
        }
        else
        {
            //if (firstTimePlay)
            //{
            //    //Ask user to tap on screen
            //    firstTimePlay = false;
            //}

            //rover.SetActive(true);
        }
    }

    private IEnumerator WaitAndTurnOnLayers()
    {
        yield return new WaitForSeconds(1f);

        mainARCamera.cullingMask &= 0x00000000;
        mainARCamera.cullingMask |= ~(0x00000000);

        //mainARCamera.cullingMask |= 1 << LayerMask.NameToLayer("ARSimulation");
    }


}
