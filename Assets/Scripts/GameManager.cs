using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager instance;

    static GameObject blue, red;

    public static int currChunk = 1;
    public static GameObject chunkObj;
    public static bool lastChildAppeared = false;
    public static float lastChildYpos = 0;

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

        AudioManager.PlayAudio("Collision");
        ball.GetComponent<Renderer>().enabled = false;
        ball.GetComponent<Collider2D>().enabled = false;
        ball.GetComponent<ParticleSystem>().Play();
        // other ball already fired this function
        if (otherBall.GetComponent<Movement>().enabled == false) return;


        otherBall.GetComponent<Movement>().enabled = false;
        ball.GetComponent<Movement>().enabled = false;



        instance.GetComponent<ChunkGenerator>().enabled = false;

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

        blue.GetComponent<Collider2D>().enabled = false;
        red.GetComponent<Collider2D>().enabled = false;
        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
        }

        instance.StartCoroutine(ResetObjects());
        instance.StartCoroutine(ResetBalls());


    }

    private static IEnumerator ResetObjects()
    {

        yield return new WaitForSeconds(1);
        GameObject[] Chunks = GameObject.FindGameObjectsWithTag("Chunk");
        foreach (var chunk in Chunks)
        {
            Destroy(chunk);
        }
        lastChildYpos = 0;
        instance.GetComponent<ChunkGenerator>().enabled = true;


    }

    private static IEnumerator ResetBalls()
    {

        Movement blueMovement, redMovement;
        blueMovement = blue.GetComponent<Movement>();
        redMovement = red.GetComponent<Movement>();
        blue.GetComponent<Renderer>().enabled = true;
        red.GetComponent<Renderer>().enabled = true;



        int counter = 1;
        if (blueMovement.currDirection == Direction.COUNTERCLOCKWISE)
        {
            MovementSync.rotationSpeed = blueMovement.angle + 360;

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
                    MovementSync.rotationSpeed = MovementSync.defaultRotationSpeed;

                    break;
                }
            }
        }
        else
        {
            MovementSync.rotationSpeed = 720 - blueMovement.angle;

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
                    MovementSync.rotationSpeed = MovementSync.defaultRotationSpeed;

                    break;
                }
            }
        }




    }

}
