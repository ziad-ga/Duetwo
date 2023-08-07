using UnityEngine;

public class Options : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = ((int)Screen.currentResolution.refreshRate);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
