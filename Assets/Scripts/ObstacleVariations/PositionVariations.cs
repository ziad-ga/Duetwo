using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionVariations : MonoBehaviour
{
    [SerializeField][Tooltip("Possible values of starting position")]
    private float[] positions = new float[2];
    void Awake()
    {
        transform.position = new Vector3(positions[Random.Range(0, positions.Length)], transform.position.y, transform.position.z);
    }
}
