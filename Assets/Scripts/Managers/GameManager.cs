using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private PlayerMovement playerMovement;
    private ObstacleGenerator obstacleGenerator;

    [SerializeField]
    private bool _isResetting = false;
    [SerializeField]
    private bool _inPlayMode = false;
    [SerializeField]
    private float _gameSpeed = 1; // speed multiplier for all game objects
    [SerializeField]
    private bool _lastObstacleAppeared = false;
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _score = 0;

    public static bool IsResetting { get { return instance._isResetting; } }
    public static bool InPlayMode { get { return instance._inPlayMode; } }
    public static float GameSpeed { get { return instance._gameSpeed; } }
    public static bool LastObstacleAppeared { get { return instance._lastObstacleAppeared; } set { instance._lastObstacleAppeared = value; } }
    public static float HP { get { return instance._hp; } }
    public static float Score { get { return instance._score; } }
    public static float GameUpdateInterval { get { return Defaults.GAME_UPDATE_INTERVAL; } }
    public static float Clock = 0;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        obstacleGenerator = GetComponent<ObstacleGenerator>();
        _hp = Defaults.HP;
        _score = 0;
    }

    private void Start()
    {
        TransitionToMenu(false);
        // StartCoroutine(IncreaseGameSpeed());
        // StartCoroutine(UpdateStats());
    }

    /// <summary>
    /// Update internal score and HP every frame
    /// </summary>
    private IEnumerator UpdateStats()
    {
        while (true)
        {
            yield return null;
            yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);
            Clock += Time.deltaTime;
            if (Math.Round(Clock, 2) == GameUpdateInterval) Clock = 5;
            Clock = Clock % GameUpdateInterval;
            _score = _score + Mathf.Exp(Time.deltaTime * _gameSpeed * 40) * 0.1f * Time.timeScale;
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
            yield return new WaitUntil(() => !GameManager.IsResetting && GameManager.InPlayMode);

            if (!Mathf.Approximately(_gameSpeed, 3f)) _gameSpeed += 0.1f;
            else break;
        }
    }

    /// <summary>
    /// Push all obstacles above the screen and continue playing
    /// </summary>
    private IEnumerator ClearScreen(GameObject[] obstacles)
    {
        yield return new WaitForSeconds(1);

        foreach (var collider in playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        GameObject firstObstacle = obstacles.OrderBy(x => x.transform.position.y).First();
        float resetSpeed = Utility.GetScreenTopBottomBoundries().x - firstObstacle.transform.position.y;
        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, resetSpeed);
        }

        instance.StartCoroutine(ResetPlayer());

        yield return new WaitForSeconds(1);

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Defaults.NORMAL_OBSTACLE_SPEED * GameManager.GameSpeed * Defaults.NORMAL_OBSTACLE_SPEED_MULTIPLIER);
        }

        instance.obstacleGenerator.enabled = true;

    }

    /// <summary>
    /// Destroy all obstacles and enable obstacle generator(optional)
    /// </summary>
    private IEnumerator DestroyObstacles(bool generateAgain = true)
    {
        yield return new WaitForSeconds(1);

        GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in Obstacles)
        {
            Destroy(obstacle);
        }

        if (generateAgain) obstacleGenerator.enabled = true;
        else obstacleGenerator.enabled = false;
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

        Direction resetDirection = (playerMovement.currDirection == Direction.COUNTERCLOCKWISE) ? Direction.CLOCKWISE : Direction.COUNTERCLOCKWISE;
        float speed = (playerMovement.currDirection == Direction.COUNTERCLOCKWISE) ? playerMovement.Angle + 360 : 720 - playerMovement.Angle;
        Func<float, float, bool> ContinueSpinCondition = (playerMovement.currDirection == Direction.COUNTERCLOCKWISE) ? (newAngle, oldAngle) => newAngle > oldAngle : (newAngle, oldAngle) => newAngle < oldAngle;

        while (true)
        {
            yield return null;

            float tempAngle = playerMovement.Angle;
            playerMovement.Rotate(resetDirection, speed);
            if (ContinueSpinCondition(playerMovement.Angle, tempAngle))
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

                playerMovement.rotationSpeed = Defaults.BALL_ROTATION_SPEED * GameManager.GameSpeed * Defaults.BALL_ROTATION_SPEED_MULTIPLIER;
                playerMovement.enabled = true;

                instance._isResetting = false;

                break;
            }
        }
    }
    private IEnumerator EnableRestartPrompt()
    {
        yield return new WaitForSeconds(1);
        UI.EnableRestartPrompt();
    }
    /// <summary>
    /// Put game in a resetting state and decide whether to restart the game or only reset current chunks
    /// </summary>
    public static void HandleCollision()
    {
        // other ball already fired this function
        if (instance.playerMovement.enabled == false) return;

        instance._isResetting = true;
        instance._hp -= Defaults.HIT_DAMAGE;

        instance._gameSpeed = Mathf.Clamp(instance._gameSpeed - 0.3f, 1, 3);

        instance.playerMovement.enabled = false;
        instance.obstacleGenerator.enabled = false; // stop generating chunks until we clear the screen

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (instance._hp > 0) instance.StartCoroutine(instance.ClearScreen(obstacles));
        else
        {
            if (instance._score > PlayerPrefs.GetInt("Highscore", 0)) PlayerPrefs.SetInt("Highscore", (int)instance._score);

            instance.StartCoroutine(instance.EnableRestartPrompt());
        }
    }
    /// <summary>
    /// Reset game to initial state
    /// </summary>
    public static void RestartGame()
    {
        instance._score = 0;
        instance._gameSpeed = 1;
        instance._hp = Defaults.HP;

        foreach (var collider in instance.playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 30);
        }
        instance.StartCoroutine(instance.DestroyObstacles());
        instance.StartCoroutine(instance.ResetPlayer());
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public static void TransitionToMenu(bool playAnimation = true)
    {
        if (instance._score > PlayerPrefs.GetInt("Highscore", 0)) PlayerPrefs.SetInt("Highscore", (int)instance._score);

        instance._inPlayMode = false;
        instance.StopAllCoroutines();
        instance._hp = 0;
        Time.timeScale = 1;
        foreach (var collider in instance.playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        foreach (var renderer in instance.playerMovement.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            obstacle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 30);
        }
        instance.playerMovement.enabled = false;
        if (playAnimation)
        {

            instance.playerMovement.transform.DORotate(new Vector3(0, 0, -600), 1f, RotateMode.LocalAxisAdd).SetEase(Ease.InSine).OnComplete(() => instance.StartCoroutine(instance.RotatePlayerInMenu()));
            instance.playerMovement.transform.DOMove(new Vector3(0, 0), 0.8f).SetEase(Ease.InSine);
        }
        else
        {
            instance.StartCoroutine(instance.RotatePlayerInMenu());
        }

        instance.StartCoroutine(instance.DestroyObstacles(false));
        UI.EnableMenu();
    }
    public static void StartGame()
    {
        instance.StopAllCoroutines();
        UI.DisableMenu();
        instance.StartCoroutine(instance.IncreaseGameSpeed());
        instance.StartCoroutine(instance.UpdateStats());
        instance._inPlayMode = true;
        instance._isResetting = false;
        instance._hp = Defaults.HP;
        instance._score = 0;
        instance._gameSpeed = 1;
        foreach (var collider in instance.playerMovement.GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = true;
        }
        instance.playerMovement.transform.DOMove(new Vector3(0, -3), 0.8f);
        instance.playerMovement.transform.DORotate(new Vector3(0, 0, -(360 + instance.playerMovement.Angle)), 0.75f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            instance.playerMovement.enabled = true;
            instance.obstacleGenerator.enabled = true;

        });



    }
    private IEnumerator RotatePlayerInMenu()
    {
        while (true)
        {
            yield return null;
            playerMovement.Rotate(Direction.CLOCKWISE, 200);
        }
    }
}
