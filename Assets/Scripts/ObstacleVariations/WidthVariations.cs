using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthVariations : MonoBehaviour
{
    public float[] widths = new float[2];
    void Awake()
    {
        transform.localScale = new Vector3(widths[Random.Range(0, widths.Length)], transform.localScale.y, transform.localScale.z);
    }
}
