using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
