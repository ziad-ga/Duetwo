using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1f; // set from inspector

    void Start()
    {
        bool active = PlayerPrefs.GetInt("Background", 1) == 1 ? true : false;
        gameObject.SetActive(active);

    }
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.unscaledDeltaTime);
    }
}
