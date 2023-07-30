using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{

    [SerializeField]
    private float angularVelocity;
    void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
    }

}
