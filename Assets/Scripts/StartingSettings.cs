using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingSettings : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().isOn = PlayerPrefs.GetInt(name, 1) == 1;
    }

}
