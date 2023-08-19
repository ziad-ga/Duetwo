using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
public class RotatingObstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speedMultiplier = Defaults.ROTATING_OBSTACLE_SPEED_MULTIPLIER;
    private float maxRotationSpeed = 450f;
    private int rotationDirection = 1;
    [SerializeField]
    private float rotationSpeed = 150f;
    private bool IsResetting = false;
    void Start()
    {
        rotationDirection = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, rotationSpeed * GameManager.GameSpeed * speedMultiplier);

        // StartCoroutine(UpdateAngularVelocity());
    }
    private void Update()
    {
        if (!GameManager.IsResetting && GameManager.InPlayMode)
            rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, rotationSpeed * GameManager.GameSpeed * speedMultiplier);
        if (rb.velocity.y > 0 && !IsResetting)
        {
            IsResetting = true;
            rb.angularVelocity = 0;
            transform.DORotate(new Vector3(0, 0, 0), 1f).OnComplete(() =>
            {
                IsResetting = false;
                rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, rotationSpeed * GameManager.GameSpeed * speedMultiplier);
            });
        }
    }
    /// <summary>
    /// Update angular velocity of the obstacle every game speed update interval
    /// </summary>
    // private IEnumerator UpdateAngularVelocity()
    // {
    //     while (true)
    //     {
    //         yield return new WaitUntil(() => Math.Round(GameManager.Clock, 2) == GameManager.GameUpdateInterval);
    //         yield return null; // wait for game manager to update game speed
    //         yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);

    //         rb.angularVelocity = rotationDirection * MathF.Min(maxRotationSpeed, rotationSpeed * GameManager.GameSpeed * speedMultiplier);
    //     }
    // }


}
