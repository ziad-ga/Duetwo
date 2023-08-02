using System.Collections;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    static GameManager instance;
    static GameObject playerObj;
    public static bool lastChildAppeared = false;
    public static float lastChildYpos = 0;
    public static float gameSpeed = 1; // speed multiplier for all game objects
    public static float gameUpdateInterval = 5; // update game speed every x seconds



    private static float score = 0;
    private static Coroutine updateScoreCoroutine;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        UI.scoreText.text = "0";
        score = 0;

        StartCoroutine(IncreaseGameSpeed());
        updateScoreCoroutine = StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return null;

            score = score + Mathf.Exp(Time.deltaTime * gameSpeed * 40) * 0.1f;
            UI.scoreText.text = ((int)score).ToString();
        }
    }

    private IEnumerator IncreaseGameSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(gameUpdateInterval);

            if (!Mathf.Approximately(gameSpeed, 3f)) gameSpeed += 0.1f;
            else break;
        }
    }

    public static void HandleCollision()
    {
        // other ball already fired this function
        if (playerObj.GetComponent<PlayerMovement>().enabled == false) return;

        gameSpeed = 1;
        score = 0;
        instance.StopCoroutine(updateScoreCoroutine);

        playerObj.GetComponent<PlayerMovement>().enabled = false;
        instance.GetComponent<ChunkGenerator>().enabled = false; // stop generating chunks until we clear the screen

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            obstacle.GetComponent<DefaultObstacleMovement>().StopAllCoroutines();

            // for obstacles with multiple scripts i.e. rotating obstacles
            var scripts = obstacle.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                script.StopAllCoroutines();
            }

        }

        instance.StartCoroutine(RestartGame(obstacles));
    }

    private static IEnumerator RestartGame(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        UI.scoreText.text = "0";

        foreach (var collider in playerObj.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15);
        }

        instance.StartCoroutine(ResetChunks());
        instance.StartCoroutine(ResetPlayer());

    }

    /// <summary>
    /// Destroy all chunks and enable chunk generator
    /// </summary>
    private static IEnumerator ResetChunks()
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

    /// <summary>
    /// Reset player smoothly
    /// </summary>
    private static IEnumerator ResetPlayer()
    {


        foreach (var renderer in playerObj.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        PlayerMovement playerMovement = playerObj.GetComponent<PlayerMovement>();
        float OrgRotationSpeed = PlayerMovement.rotationSpeed;

        int counter = 1;

        if (PlayerMovement.currDirection == Direction.COUNTERCLOCKWISE)
        {

            PlayerMovement.rotationSpeed = playerMovement.angle + 360;

            while (true)
            {
                yield return null;

                float tempAngle = playerMovement.angle;
                playerMovement.Rotate(Direction.CLOCKWISE);
                if (playerMovement.angle > tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    foreach (var collider in playerObj.GetComponentsInChildren<Collider2D>())
                    {
                        collider.enabled = true;
                    }

                    // reset rotation to exactly 0
                    playerMovement.transform.Rotate(new Vector3(0, 0, -playerMovement.transform.rotation.eulerAngles.z)); 

                    PlayerMovement.rotationSpeed = OrgRotationSpeed;
                    playerMovement.enabled = true;
                    
                    updateScoreCoroutine = instance.StartCoroutine("UpdateScore");

                    break;
                }
            }
        }
        else
        {
            PlayerMovement.rotationSpeed = 720 - playerMovement.angle;

            while (true)
            {
                yield return null;

                float tempAngle = playerMovement.angle;
                playerMovement.Rotate(Direction.COUNTERCLOCKWISE);
                if (playerMovement.angle < tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    foreach (var collider in playerObj.GetComponentsInChildren<Collider2D>())
                    {
                        collider.enabled = true;
                    }

                    // reset rotation to exactly 0
                    playerMovement.transform.Rotate(new Vector3(0, 0, -playerMovement.transform.rotation.eulerAngles.z));

                    PlayerMovement.rotationSpeed = OrgRotationSpeed;
                    playerMovement.enabled = true;

                    updateScoreCoroutine = instance.StartCoroutine("UpdateScore");

                    break;
                }
            }
        }




    }
}
