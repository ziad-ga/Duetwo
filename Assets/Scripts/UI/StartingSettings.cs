using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingSettings : MonoBehaviour
{
    [SerializeField]
    private bool defaultValue; // set from inspector
    void Start()
    {
        GetComponent<Toggle>().isOn = PlayerPrefs.GetInt(name, defaultValue ? 1 : 0) == 1;
    }

}
