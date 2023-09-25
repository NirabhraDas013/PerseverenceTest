using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    VisualElement rootElement;

    //UI Containers
    VisualElement roverMovementButtonsContainer;
    VisualElement roverArmControlUIContainer;
    VisualElement roverArmControlButtonsContainer;
    VisualElement startPlaneGenerationButtonsContainer;
    VisualElement startPlayingButtonContainer;
    VisualElement promptPlayerContainer;


    //Rover Movement Buttons
    Button moveForwardButton;
    Button moveReverseButton;
    Button turnLeftButton;
    Button turnRightButton;

    //Rover Arm Control Switch Button
    Button roverArmControlModeButton;

    //Rover Arm Control Buttons
    Button roverArmMoveLeftButton;
    Button roverArmMoveRightButton;
    Button roverArmMoveUpButton;
    Button roverArmMoveDownButton;
    Button roverArmMoveFrontButton;
    Button roverArmMoveBackButton;

    //Play Mode Switch Buttons
    Button startPlaying;
    Button generatePlane;

    VisualElement target;
    UIBUTTON uiButton;

    [SerializeField] private UIDocument mainUIDocument;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found More Than One UImanager in the Scene.");
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        rootElement = mainUIDocument.rootVisualElement;

        //Get All the Containers
        roverMovementButtonsContainer = rootElement.Q<VisualElement>("RoverMovementButtonsContainer");
        roverArmControlUIContainer = rootElement.Q<VisualElement>("RoverArmControlUIContainer");
        roverArmControlButtonsContainer = rootElement.Q<VisualElement>("ArmControls");
        startPlaneGenerationButtonsContainer = rootElement.Q<VisualElement>("StartPlaneGenerationButtonContainer");
        startPlayingButtonContainer = rootElement.Q<VisualElement>("StartPlayingButtonContainer");
        promptPlayerContainer = rootElement.Q<VisualElement>("PromptPlayerContainer");


        //Rover Driving Control Buttons
        moveForwardButton = rootElement.Q<Button>("DriveForwardButton");
        moveReverseButton = rootElement.Q<Button>("DriveBackButton");
        turnLeftButton = rootElement.Q<Button>("TurnLeftButton");
        turnRightButton = rootElement.Q<Button>("TurnRightButton");

        //Rover Arm Control
        roverArmControlModeButton = rootElement.Q<Button>("ControlModeButton");

        //Rover Arm Control Buttons
        roverArmMoveLeftButton = rootElement.Q<Button>("ArmLeft");
        roverArmMoveRightButton = rootElement.Q<Button>("ArmRight");
        roverArmMoveUpButton = rootElement.Q<Button>("ArmUp");
        roverArmMoveDownButton = rootElement.Q<Button>("ArmDown");
        roverArmMoveFrontButton = rootElement.Q<Button>("ArmFront");
        roverArmMoveBackButton = rootElement.Q<Button>("ArmBack");

        //PlayMode Switch Buttons
        startPlaying = rootElement.Q<Button>("StartPlayingButton");
        generatePlane = rootElement.Q<Button>("GeneratePlaneButton");

        //all buttons should be non-clickable for the click and hold to work
        //Rover Movement
        moveForwardButton.clickable = null;
        moveReverseButton.clickable = null;
        turnLeftButton.clickable = null;
        turnRightButton.clickable = null;
        //Rover Arm
        roverArmMoveLeftButton.clickable = null;
        roverArmMoveRightButton.clickable = null;
        roverArmMoveUpButton.clickable = null;
        roverArmMoveDownButton.clickable = null;
        roverArmMoveFrontButton.clickable = null;
        roverArmMoveBackButton.clickable = null;


        GameEventsManager.instance.MiscEvents.OnStartGame += StartGameUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.MiscEvents.OnStartGame -= StartGameUI;
    }

    private void Start()
    {
        //Add Click And Hold to all UI buttons applicable
        //Rover Movement
        new ClickAndHoldButton(moveForwardButton, UIBUTTON.FORWARD);
        new ClickAndHoldButton(moveReverseButton, UIBUTTON.REVERSE);
        new ClickAndHoldButton(turnLeftButton, UIBUTTON.LEFT);
        new ClickAndHoldButton(turnRightButton, UIBUTTON.RIGHT);
        //Rover Arm
        new ClickAndHoldButton(roverArmMoveLeftButton, UIBUTTON.ARMLEFT);
        new ClickAndHoldButton(roverArmMoveRightButton, UIBUTTON.ARMRIGHT);
        new ClickAndHoldButton(roverArmMoveUpButton, UIBUTTON.ARMUP);
        new ClickAndHoldButton(roverArmMoveDownButton, UIBUTTON.ARMDOWN);
        new ClickAndHoldButton(roverArmMoveFrontButton, UIBUTTON.ARMFRONT);
        new ClickAndHoldButton(roverArmMoveBackButton, UIBUTTON.ARMBACK);

        //Set Initial Rover Arm Control UI
        SetRoverArmControlUI();

        //Set all Click Button
        roverArmControlModeButton.clicked += SwitchArmControlModeButtonClicked;
        generatePlane.clicked += () => SwitchPlayMode(true);
        startPlaying.clicked += () => SwitchPlayMode(false);

        //Set Play Mode to Plane Generation at start
        SwitchPlayMode(true);
        promptPlayerContainer.visible = false;
    }

    private void SwitchArmControlModeButtonClicked()
    {
        GameEventsManager.instance.UIEvents.ButtonDown(UIBUTTON.CONTROLMODE);

        SetRoverArmControlUI();
    }

    private void SetRoverArmControlUI()
    {
        if (GameController.instance.GetRoverControlMode())
        {
            roverArmControlModeButton.text = "Automatic";
            roverArmControlButtonsContainer.visible = false;
        }
        else
        {
            roverArmControlModeButton.text = "Manual";
            roverArmControlButtonsContainer.visible = true;
        }
    }

    private void SwitchPlayMode(bool generatePlane)
    {
        //if (doesPlayerWantGeneratePlane)
        //{
        //    roverMovementButtonsContainer.visible = false;
        //    roverArmControlUIContainer.visible = false;
        //    roverArmControlButtonsContainer.visible = false;
        //    startPlaneGenerationButtonsContainer.visible = false;

        //    startPlayingButtonContainer.visible = true;
        //}
        //else
        //{
        //    roverMovementButtonsContainer.visible = !doesPlayerWantGeneratePlane;
        //    roverArmControlUIContainer.visible = !doesPlayerWantGeneratePlane;
        //    roverArmControlButtonsContainer.visible = !doesPlayerWantGeneratePlane;
        //    startPlaneGenerationButtonsContainer.visible = !doesPlayerWantGeneratePlane;

        //    startPlayingButtonContainer.visible = doesPlayerWantGeneratePlane;
        //}

        roverMovementButtonsContainer.visible = !generatePlane;
        roverArmControlUIContainer.visible = !generatePlane;
        roverArmControlButtonsContainer.visible = !generatePlane;
        startPlaneGenerationButtonsContainer.visible = !generatePlane;

        startPlayingButtonContainer.visible = generatePlane;
        promptPlayerContainer.visible = generatePlane;

        GameEventsManager.instance.InputEvents.SwitchPlayMode(generatePlane);
    }

    private void StartGameUI()
    {
        roverMovementButtonsContainer.visible = true;
        roverArmControlUIContainer.visible = true;
        roverArmControlButtonsContainer.visible = true;
        startPlaneGenerationButtonsContainer.visible = true;

        startPlayingButtonContainer.visible = false;
        promptPlayerContainer.visible = false;
    }

    private IEnumerator FlashUserPrompt()
    {
        promptPlayerContainer.visible = true;
        yield return new WaitForSeconds(2f);
        promptPlayerContainer.visible = false;
    }
}
