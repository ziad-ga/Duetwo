using UnityEngine;
using System;
using System.Collections;
public class RotatingObstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speedMultiplier = Defaults.ROTATING_OBSTACLE_SPEED_MULTIPLIER;
    private float maxRotationSpeed = 450f;
    private int rotationDirection = 1;

    void Start()
    {
        rotationDirection = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, Defaults.ROTATING_OBSTACLE_SPEED * GameManager.GameSpeed * speedMultiplier);

        StartCoroutine(UpdateAngularVelocity());
    }
    /// <summary>
    /// Update angular velocity of the obstacle every game speed update interval
    /// </summary>
    private IEnumerator UpdateAngularVelocity()
    {
        while (true)
        {
            yield return new WaitUntil(() => Math.Round(GameManager.Clock, 2) == GameManager.GameUpdateInterval);
            yield return null; // wait for game manager to update game speed
            yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);

            rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, Defaults.ROTATING_OBSTACLE_SPEED * GameManager.GameSpeed * speedMultiplier);
        }
    }


}
