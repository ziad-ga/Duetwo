using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthVariations : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Possible values of starting width")]
    private float[] widths = new float[2];
    void Awake()
    {
        transform.localScale = new Vector3(widths[Random.Range(0, widths.Length)], transform.localScale.y, transform.localScale.z);
    }
}
