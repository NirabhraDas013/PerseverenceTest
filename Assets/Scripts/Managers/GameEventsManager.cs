using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public UIEvents UIEvents;
    public InputEvents InputEvents;
    public MiscEvents MiscEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found More Than One GameEventsManager in the Scene.");
            Destroy(this);
        }
        instance = this;

        //Initialize all Events
        UIEvents = new UIEvents();
        InputEvents = new InputEvents();
        MiscEvents = new MiscEvents();
    }
}
