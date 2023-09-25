using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelInfo
{
    public GameObject wheel;
    public bool isDrive;
    public bool isTurn;
    [Range(-1, 1)]public int turnDirection;

    public WheelInfo(GameObject wheel, bool isDrive, bool isTurn, int turnDirection)
    {
        this.wheel = wheel;
        this.isDrive = isDrive;
        this.isTurn = isTurn;
        this.turnDirection = turnDirection;
    }
}
