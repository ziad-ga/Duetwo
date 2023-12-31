using UnityEngine;
using System;
using System.Collections;
public class DefaultObstacleMovement : MonoBehaviour
{

    private bool isOnScreen = false;
    private bool alreadyAppeared = false;
    private Rigidbody2D rb;
    private float speedMultiplier = Defaults.NORMAL_OBSTACLE_SPEED_MULTIPLIER;
    public ObstacleType type; // set from the inspector
    void Start()
    {
        if (PlayerPrefs.GetInt("VFX", 1) == 0)
        {
            GetComponent<TrailRenderer>().enabled = false;
        }
        rb = GetComponent<Rigidbody2D>();

        // StartCoroutine(UpdateSpeed());
    }

    private void Update()
    {
        if (!GameManager.IsResetting && GameManager.InPlayMode)
            rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed * speedMultiplier);
        // destroy obstacle if it is not visible anymore
        if (!GetComponent<Renderer>().isVisible && isOnScreen)
        {
            // destroy obstacle if its last child is not visible anymore and is below the screen
            if (Camera.main.WorldToScreenPoint(transform.position).y < 0) Destroy(gameObject);

        }
        // if obstacle appears on the screen for the first time
        if (GetComponent<Renderer>().isVisible && !isOnScreen)
        {
            isOnScreen = true;
            if (!alreadyAppeared)
            {
                alreadyAppeared = true;
                GameManager.LastObstacleAppeared = true;
            }

        }
    }
    // private IEnumerator UpdateSpeed()
    // {
    //     while (true)
    //     {
    //         yield return new WaitUntil(() => Math.Round(GameManager.Clock, 2) == GameManager.GameUpdateInterval);
    //         yield return null; // wait for game manager to update game speed
    //         yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);

    //         rb.velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed * speedMultiplier);
    //     }
    // }
}
