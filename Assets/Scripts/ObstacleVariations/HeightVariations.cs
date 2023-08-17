using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightVariations : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Possible values of starting height")]
    private float[] heights = new float[2];
    void Awake()
    {
        transform.localScale = new Vector3(transform.localScale.x, heights[Random.Range(0, heights.Length)], transform.localScale.z);
    }
}
