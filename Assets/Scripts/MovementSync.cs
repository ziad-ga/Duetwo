using UnityEngine;
using System.Collections;
public class MovementSync : MonoBehaviour
{
    public static float rotationSpeed = Defaults.BALL_ROTATION_SPEED  * GameManager.gameSpeed;

    public static float radius = Defaults.BALL_ROTATION_RADIUS;

    public static float height = Defaults.BALL_HEIGHT;

    private void Start()
    {
        StartCoroutine(UpdateRotationSpeed());
    }
    private IEnumerator UpdateRotationSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(GameManager.gameUpdateInterval);
            yield return null; // wait for game manager to update game speed
            rotationSpeed = Defaults.BALL_ROTATION_SPEED * GameManager.gameSpeed;
        }
    }
}
