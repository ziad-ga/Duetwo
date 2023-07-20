using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObstacleMovement : MonoBehaviour
{

    public float ms;


    private bool isOnScreen = false;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -ms);
    }


    private void Update()
    {
        if (!GetComponent<Renderer>().isVisible && isOnScreen)
        {
            Destroy(gameObject);
        }
        if (GetComponent<Renderer>().isVisible && !isOnScreen)
        {
            isOnScreen = true;
        }
    }
}
