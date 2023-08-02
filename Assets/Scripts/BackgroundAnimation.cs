using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    public float rotationSpeed = 1f; // Set from inspector

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
    }
}
