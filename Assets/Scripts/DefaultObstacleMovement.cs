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
        rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed);

        StartCoroutine(UpdateSpeed());
    }
    
    private void Update()
    {
        // destroy obstacle if it is not visible anymore
        if (!GetComponent<Renderer>().isVisible && isOnScreen)
        {
            // destroy parent if this is the last obstacle in the chunk
            if (transform.parent.childCount == 1) Destroy(transform.parent.gameObject); 

            Destroy(gameObject);
        }
        // If obstacle appears on the screen for the first time
        if (GetComponent<Renderer>().isVisible && !isOnScreen)
        {
            isOnScreen = true;
            if (isLastChild)
            {
                GameManager.LastChildAppeared = true;
                GameManager.LastChildYpos = transform.position.y;
            }

        }
    }
    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.GameUpdateInterval);
            yield return null; // wait for game manager to update game speed

            rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed);
        }
    }
}
