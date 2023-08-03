using System.Collections;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Coroutine updateScoreCoroutine;
    private PlayerMovement playerMovement;

    [SerializeField]
    private float score = 0;

    [SerializeField]
    private float _gameSpeed = 1; // speed multiplier for all game objects
    [SerializeField]
    private float _gameUpdateInterval = 5; // update game speed every x seconds
    [SerializeField]
    private bool _lastChildAppeared = false;
    [SerializeField]
    private float _lastChildYpos = 0;


    public static float GameSpeed { get { return instance._gameSpeed; } set { instance._gameSpeed = value; } }
    public static float GameUpdateInterval { get { return instance._gameUpdateInterval; } }
    public static bool LastChildAppeared { get { return instance._lastChildAppeared; } set { instance._lastChildAppeared = value; } }
    public static float LastChildYpos { get { return instance._lastChildYpos; } set { instance._lastChildYpos = value; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        UI.ScoreText.text = "0";
        score = 0;

        StartCoroutine(IncreaseGameSpeed());
        updateScoreCoroutine = StartCoroutine(UpdateScore());
    }

    /// <summary>
    /// Update internal and UI score every frame
    /// </summary>
    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return null;

            score = score + Mathf.Exp(Time.deltaTime * _gameSpeed * 40) * 0.1f;
            UI.ScoreText.text = ((int)score).ToString();
        }
    }


    /// <summary>
    /// Update game speed every interval seconds
    /// </summary>
    private IEnumerator IncreaseGameSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(_gameUpdateInterval);

            if (!Mathf.Approximately(_gameSpeed, 3f)) _gameSpeed += 0.1f;
            else break;
        }
    }

    public static void HandleCollision()
    {
        // other ball already fired this function
        if (instance.playerMovement.enabled == false) return;

        GameSpeed = 1;
        instance.score = 0;
        instance.StopCoroutine(instance.updateScoreCoroutine);

        instance.playerMovement.enabled = false;
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

        instance.StartCoroutine(instance.RestartGame(obstacles));
    }

    private IEnumerator RestartGame(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        UI.ScoreText.text = "0";

        foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
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
    private IEnumerator ResetChunks()
    {

        yield return new WaitForSeconds(1);

        GameObject[] Chunks = GameObject.FindGameObjectsWithTag("Chunk");
        foreach (var chunk in Chunks)
        {
            Destroy(chunk);
        }
        LastChildYpos = 0;
        instance.GetComponent<ChunkGenerator>().enabled = true;
    }

    /// <summary>
    /// Reset player smoothly
    /// </summary>
    private IEnumerator ResetPlayer()
    {
        foreach (var renderer in playerMovement.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        int counter = 1;

        if (playerMovement.currDirection == Direction.COUNTERCLOCKWISE)
        {

            playerMovement.rotationSpeed = playerMovement.Angle + 360;

            while (true)
            {
                yield return null;

                float tempAngle = playerMovement.Angle;
                playerMovement.Rotate(Direction.CLOCKWISE);
                if (playerMovement.Angle > tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
                    {
                        collider.enabled = true;
                    }

                    // reset rotation to exactly 0
                    playerMovement.transform.Rotate(new Vector3(0, 0, -playerMovement.transform.rotation.eulerAngles.z));

                    playerMovement.rotationSpeed = Defaults.BALL_ROTATION_SPEED;
                    playerMovement.StartCoroutine(playerMovement.UpdateRotationSpeed());
                    playerMovement.enabled = true;

                    updateScoreCoroutine = instance.StartCoroutine("UpdateScore");

                    break;
                }
            }
        }
        else
        {
            playerMovement.rotationSpeed = 720 - playerMovement.Angle;

            while (true)
            {
                yield return null;

                float tempAngle = playerMovement.Angle;
                playerMovement.Rotate(Direction.COUNTERCLOCKWISE);
                if (playerMovement.Angle < tempAngle)
                {
                    if (counter > 0)
                    {
                        counter--;
                        continue;
                    }
                    foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
                    {
                        collider.enabled = true;
                    }

                    // reset rotation to exactly 0
                    playerMovement.transform.Rotate(new Vector3(0, 0, -playerMovement.transform.rotation.eulerAngles.z));

                    playerMovement.rotationSpeed = Defaults.BALL_ROTATION_SPEED;
                    playerMovement.StartCoroutine(playerMovement.UpdateRotationSpeed());
                    playerMovement.enabled = true;

                    updateScoreCoroutine = instance.StartCoroutine("UpdateScore");

                    break;
                }
            }
        }
    }
}
