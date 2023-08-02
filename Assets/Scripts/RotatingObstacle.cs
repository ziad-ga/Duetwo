using UnityEngine;
using System;
using System.Collections;
public class RotatingObstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Defaults.ROTATING_OBSTACLE_SPEED * GameManager.gameSpeed;
        
        StartCoroutine(UpdateAngularVelocity());
    }

    private IEnumerator UpdateAngularVelocity()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.gameUpdateInterval);
            yield return null; // wait for game manager to update game speed

            rb.angularVelocity = Defaults.ROTATING_OBSTACLE_SPEED * GameManager.gameSpeed;
        }
    }


}
