using UnityEngine;

public class Options : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = ((int)Screen.currentResolution.refreshRate);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public static void HandlePlayerSettings(string name, bool value)
    {
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        switch (name)
        {
            case "Music":
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>().mute = !value;
                break;
            case "SoundEffects":
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>().mute = !value;

                break;
            case "InvertColors":

                break;
            case "VisualEffects":

                break;
            case "InvertControls":

                break;
            default: Debug.Log("Invalid player setting name: " + name); break;
        }
    }
}
