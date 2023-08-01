using UnityEngine;
using System;
using System.Collections;
public class DefaultObstacleMovement : MonoBehaviour
{

    public bool isLastChild = false;

    private bool isOnScreen = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.gameSpeed);
        StartCoroutine(UpdateSpeed());
    }
    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.gameUpdateInterval);
            yield return null; // wait for game manager to update game speed
            rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.gameSpeed);
        }
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
