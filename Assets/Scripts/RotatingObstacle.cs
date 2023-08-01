using UnityEngine;
using System;
public class RotatingObstacle : MonoBehaviour
{
    [NonSerialized]
    public float angularVelocity = Defaults.ROTATING_OBSTACLE_SPEED;
    void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
    }

}
