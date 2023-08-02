using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip collisionAudio;

    private AudioSource audioSource;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }
    
    /// <summary>
    /// Plays the audio clip with the given name
    /// </summary>
    public static void PlayAudio(string name)
    {
        switch (name)
        {
            case "Collision":
                instance.audioSource.PlayOneShot(instance.collisionAudio);
                break;
            default:
                Debug.Log("No audio found for " + name);
                break;
        }
    }
}
