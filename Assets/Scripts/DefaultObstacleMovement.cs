using UnityEngine;
using System;
public class DefaultObstacleMovement : MonoBehaviour
{

    public bool isLastChild = false;
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
            if (transform.parent.childCount == 1) Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
        if (GetComponent<Renderer>().isVisible && !isOnScreen)
        {
            isOnScreen = true;
            if (isLastChild)
            {
                GameManager.lastChildAppeared = true;
                GameManager.lastChildYpos = transform.position.y;
            }

        }
    }
}
