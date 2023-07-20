using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager instance;

    static GameObject blue, red;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        blue = GameObject.FindGameObjectWithTag("Blue");
        red = GameObject.FindGameObjectWithTag("Red");
    }
    public static void HandleCollision(string name)
    {
        GameObject ball = red, otherBall = blue;

        if (name == "Blue")
        {
            ball = blue;
            otherBall = red;
        }

        otherBall.GetComponent<Movement>().enabled = false;
        ball.GetComponent<Movement>().enabled = false;

        ball.SetActive(false);

        // GameObject.Destroy(ball.gameObject);

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        instance.StartCoroutine(RestartGame(obstacles));
    }

    private static IEnumerator RestartGame(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
        }

        instance.StartCoroutine(ResetObjects(obstacles));
        instance.StartCoroutine(ResetBalls());


    }

    private static IEnumerator ResetObjects(GameObject[] obstacles)
    {

        yield return new WaitForSeconds(1);

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -obstacle.GetComponent<DefaultObstacleMovement>().ms);
        }
        foreach (var obstacle in obstacles)
        {
            obstacle.transform.position = obstacle.GetComponent<DefaultObstacleMovement>().startPos;
        }


    }

    private static IEnumerator ResetBalls()
    {

        Movement blueMovement, redMovement;
        blueMovement = blue.GetComponent<Movement>();
        redMovement = red.GetComponent<Movement>();
        blue.SetActive(true);
        red.SetActive(true);
        blue.GetComponent<Collider2D>().enabled = false;
        red.GetComponent<Collider2D>().enabled = false;

        // blueMovement.RotationSpeed *= 3;

        int counter = 1;
        if (blueMovement.currDirection == Direction.COUNTERCLOCKWISE)
        {
            blueMovement.RotationSpeed = blueMovement.angle + 360;

            while (true)
            {
                yield return null;
                float tempAngle = blueMovement.angle;
                blueMovement.RotateClockWise();
                redMovement.RotateClockWise();
                if (blueMovement.angle > tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    blue.GetComponent<Movement>().enabled = true;
                    blue.GetComponent<Collider2D>().enabled = true;
                    red.GetComponent<Movement>().enabled = true;
                    red.GetComponent<Collider2D>().enabled = true;
                    // blueMovement.RotationSpeed /= 3;
                    blueMovement.RotationSpeed = MovementSync.defaultRotationSpeed;
                    break;
                }
            }
        }
        else
        {
            blueMovement.RotationSpeed = 720 - blueMovement.angle;

            while (true)
            {
                yield return null;
                float tempAngle = blueMovement.angle;
                blueMovement.RotateCounterClockWise();
                redMovement.RotateCounterClockWise();
                if (blueMovement.angle < tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    blue.GetComponent<Movement>().enabled = true;
                    blue.GetComponent<Collider2D>().enabled = true;
                    red.GetComponent<Movement>().enabled = true;
                    red.GetComponent<Collider2D>().enabled = true;
                    // blueMovement.RotationSpeed /= 3;
                    blueMovement.RotationSpeed = MovementSync.defaultRotationSpeed;



                    break;
                }
            }
        }




    }

}
