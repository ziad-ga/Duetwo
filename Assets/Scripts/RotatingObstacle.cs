using UnityEngine;
using System;
using System.Collections;
public class RotatingObstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Defaults.ROTATING_OBSTACLE_SPEED * GameManager.GameSpeed;
        
        StartCoroutine(UpdateAngularVelocity());
    }
    /// <summary>
    /// Update angular velocity of the obstacle every game speed update interval
    /// </summary>
    private IEnumerator UpdateAngularVelocity()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.GameUpdateInterval);
            yield return null; // wait for game manager to update game speed
            yield return new WaitUntil(() => !GameManager.IsResetting);

            rb.angularVelocity = Defaults.ROTATING_OBSTACLE_SPEED * GameManager.GameSpeed;
        }
    }


}
