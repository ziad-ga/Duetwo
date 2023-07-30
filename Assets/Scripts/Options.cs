using UnityEngine;

public class Options : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = ((int)Screen.currentResolution.refreshRateRatio.value);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
