using UnityEngine;
using System;
using System.Collections;
public class DefaultObstacleMovement : MonoBehaviour
{

    public bool isLastChild = false;
    private bool isOnScreen = false;
    private bool alreadyAppeared = false;
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
            // destroy chunk if its last child is not visible anymore and is below the screen
            if (isLastChild && Camera.main.WorldToScreenPoint(transform.position).y < 0) Destroy(transform.parent.gameObject);

        }
        // if obstacle appears on the screen for the first time
        if (GetComponent<Renderer>().isVisible && !isOnScreen)
        {
            isOnScreen = true;
            if (isLastChild && !alreadyAppeared)
            {
                alreadyAppeared = true;
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
            yield return new WaitUntil(() => !GameManager.IsResetting);

            rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed);
        }
    }
}
