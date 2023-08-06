using System.Collections;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Coroutine updateScoreCoroutine;
    private PlayerMovement playerMovement;

    [SerializeField]
    private float _score = 0;

    [SerializeField]
    private float _gameSpeed = 1; // speed multiplier for all game objects
    [SerializeField]
    private bool _isResetting = false;
    [SerializeField]
    private bool _lastChildAppeared = false;
    [SerializeField]
    private float _lastChildYpos = 0;
    [SerializeField]
    private float _hp;

    public static float GameSpeed { get { return instance._gameSpeed; } }
    public static float GameUpdateInterval { get { return Defaults.GAME_UPDATE_INTERVAL; } }
    public static bool IsResetting { get { return instance._isResetting; } }
    public static bool LastChildAppeared { get { return instance._lastChildAppeared; } set { instance._lastChildAppeared = value; } }
    public static float LastChildYpos { get { return instance._lastChildYpos; } set { instance._lastChildYpos = value; } }
    public static float HP { get { return instance._hp; } }
    public static float Score { get { return instance._score; } }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _hp = Defaults.HP;
    }

    private void Start()
    {
        _score = 0;

        StartCoroutine(IncreaseGameSpeed());
        updateScoreCoroutine = StartCoroutine(UpdateStats());
    }

    /// <summary>
    /// Update internal score and HP every frame
    /// </summary>
    private IEnumerator UpdateStats()
    {
        while (true)
        {
            yield return null;
            yield return new WaitUntil(() => !GameManager.IsResetting);
            _score = _score + Mathf.Exp(Time.deltaTime * _gameSpeed * 40) * 0.1f;
            _hp = Mathf.Clamp(_hp + Time.deltaTime * 5, 0, 100);
        }
    }


    /// <summary>
    /// Update game speed every interval seconds
    /// </summary>
    private IEnumerator IncreaseGameSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(Defaults.GAME_UPDATE_INTERVAL);
            yield return new WaitUntil(() => !GameManager.IsResetting);

            if (!Mathf.Approximately(_gameSpeed, 3f)) _gameSpeed += 0.1f;
            else break;
        }
    }
    /// <summary>
    /// Put game in a resetting state and decide whether to restart the game or only reset current chunks
    /// </summary>
    public static void HandleCollision()
    {
        // other ball already fired this function
        if (instance.playerMovement.enabled == false) return;

        instance._isResetting = true;
        instance._hp -= 20;

        instance._gameSpeed = Mathf.Clamp(instance._gameSpeed - 0.3f, 1, 3);

        instance.playerMovement.enabled = false;
        instance.GetComponent<ChunkGenerator>().enabled = false; // stop generating chunks until we clear the screen

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (instance._hp > 0) instance.StartCoroutine(instance.RestartChunk(obstacles));
        else instance.StartCoroutine(instance.RestartGame(obstacles));
    }

    /// <summary>
    /// Push all obstacles above the screen and continue playing
    /// </summary>
    private IEnumerator RestartChunk(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        GameObject firstObstacle = obstacles.OrderBy(x => x.transform.position.y).First();
        float resetSpeed = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - firstObstacle.transform.position.y;
        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, resetSpeed);
        }

        instance.StartCoroutine(ResetPlayer());

        yield return new WaitForSeconds(1);

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed);
        }

        instance.GetComponent<ChunkGenerator>().enabled = true;

    }
    /// <summary>
    /// Reset game to initial state
    /// </summary>
    private IEnumerator RestartGame(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        instance._score = 0;
        instance._gameSpeed = 1;
        instance._hp = Defaults.HP;

        foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 30);
        }

        instance.StartCoroutine(DestroyChunks());
        instance.StartCoroutine(ResetPlayer());

    }

    /// <summary>
    /// Destroy all chunks and enable chunk generator
    /// </summary>
    private IEnumerator DestroyChunks()
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
                    playerMovement.enabled = true;

                    instance._isResetting = false;

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

                    playerMovement.rotationSpeed = Defaults.BALL_ROTATION_SPEED * GameManager.GameSpeed;
                    playerMovement.enabled = true;
                    instance._isResetting = false;

                    break;
                }
            }
        }
    }
}
