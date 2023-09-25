using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoverWizard : EditorWindow
{
    private GameObject root;
    private GameObject body;
    private List<WheelInfo> wheelInfos = new List<WheelInfo>();
    private RoverController roverController;
    private int numberOfWheels;

    Vector2 scrollPos;
    Vector2 scrollPosWheels;
    bool showPosition = false;
    string status = "Set Up wheels";

    [MenuItem("Vehicles/Rover Wizard")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RoverWizard));
    }

    private void OnEnable()
    {
        ////Reference the Script holding the Array
        //roverController = FindObjectOfType<RoverController>();

        //if (roverController)
        //{
        //    //Create a new SerializedObject From The Script
        //    SerializedObject = new 
        //}
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        root = (GameObject)EditorGUILayout.ObjectField("Rover Root", root, typeof(GameObject), true);
        body = (GameObject)EditorGUILayout.ObjectField("Rover Body", body, typeof(GameObject), true);
        numberOfWheels = EditorGUILayout.IntField("Number of Wheels", numberOfWheels);

        showPosition = EditorGUILayout.Foldout(showPosition, status);

        if (showPosition)
        {
            //The wheels stay inside a scrollable box
            EditorGUILayout.BeginVertical();
            scrollPosWheels = EditorGUILayout.BeginScrollView(scrollPosWheels, GUILayout.Height(250));

            for (int i = 0; i < numberOfWheels; i++)
            {
                int count = wheelInfos.Count;

                GameObject wheel;
                bool isDrive;
                bool isTurn;
                int turnDirection;

                if (count > 0)
                {
                    if (i < count)
                    {
                        WheelInfo currentWheelInfo = wheelInfos[i];
                        wheel = currentWheelInfo.wheel;
                        isDrive = currentWheelInfo.isDrive;
                        isTurn = currentWheelInfo.isTurn;
                        turnDirection = currentWheelInfo.turnDirection;
                    }
                    else
                    {
                        wheel = null;
                        isDrive = false;
                        isTurn = false;
                        turnDirection = 0;
                    }
                }
                else
                {
                    wheel = null;
                    isDrive = false;
                    isTurn = false;
                    turnDirection = 0;
                }

                EditorGUILayout.LabelField($"Wheel #{i + 1}", EditorStyles.boldLabel);
                wheel = (GameObject)EditorGUILayout.ObjectField("wheel", wheel, typeof(GameObject), true);
                isDrive = EditorGUILayout.Toggle("Drive", isDrive);
                isTurn = EditorGUILayout.Toggle("Turn", isTurn);
                turnDirection = EditorGUILayout.IntSlider("Turn Direction", turnDirection, -1, 1);

                WheelInfo wheelInfo = new WheelInfo(wheel, isDrive, isTurn, turnDirection);
                wheelInfos.Add(wheelInfo);
            }

            //End Scrolling for wheels
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical(); 
        }

        //End Overall Scrolling
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
