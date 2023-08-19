using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class ObstacleGenerator : MonoBehaviour
{

    private GameObject[] normalObstacles;
    [SerializeField]
    private List<GameObject> normalPool;
    private GameObject[] hardObstacles;
    [SerializeField]
    private List<GameObject> hardPool;
    private GameObject lastObstacleObj;
    private GameObject lastObstaclePrefab;

    private int baseHardChance = 5;
    private float hardChanceExp = 2f;


    [SerializeField]
    private int hardChance;

    void Awake()
    {
        normalPool = new List<GameObject>();
        hardPool = new List<GameObject>();

        normalObstacles = (GameObject[])Resources.LoadAll("Prefabs/Obstacles/Normal").Cast<GameObject>().ToArray();
        hardObstacles = (GameObject[])Resources.LoadAll("Prefabs/Obstacles/Hard").Cast<GameObject>().ToArray();
        foreach (var obstacle in normalObstacles)
        {
            int weight = Defaults.OBSTACLE_WEIGHTS[obstacle.GetComponent<DefaultObstacleMovement>().type];
            for (int i = 0; i < weight; i++) normalPool.Add(obstacle);
        }
        foreach (var obstacle in hardObstacles)
        {
            int weight = Defaults.OBSTACLE_WEIGHTS[obstacle.GetComponent<DefaultObstacleMovement>().type];
            for (int i = 0; i < weight; i++) hardPool.Add(obstacle);
        }
    }

    private void Start()
    {
        lastObstacleObj = GetNextChunk();
    }

    private void Update()
    {
        if (lastObstacleObj == null || GameManager.LastObstacleAppeared)
        {
            GameManager.LastObstacleAppeared = false;
            lastObstacleObj = GetNextChunk();
        }
    }


    /// <summary>
    /// Returns a random obstacle from the available chunks.
    /// </summary>
    private GameObject GetNextChunk()
    {
        hardChance = (int)(baseHardChance * Mathf.Pow(GameManager.GameSpeed, hardChanceExp));
        List<GameObject> pool = Random.Range(0, 100) < 100 - hardChance ? normalPool : hardPool;
        int temp = Random.Range(0, pool.Count);

        GameObject obstaclePrefab = pool[temp];

        if (lastObstacleObj == null)
        {
            Vector3 pos = new Vector3(obstaclePrefab.transform.position.x, Utility.GetScreenTopBottomBoundries().x + obstaclePrefab.transform.localScale.y * 0.5f);
            lastObstaclePrefab = obstaclePrefab;
            return Instantiate(obstaclePrefab, pos, Quaternion.identity);
        }
        else
        {
            var padding = Mathf.Max(obstaclePrefab.transform.position.y, lastObstaclePrefab.transform.position.y);
            Vector3 pos = new Vector3(obstaclePrefab.transform.position.x, lastObstacleObj.transform.position.y + padding + obstaclePrefab.transform.localScale.y * 0.5f + lastObstaclePrefab.transform.localScale.y * 0.5f);
            lastObstaclePrefab = obstaclePrefab;
            return Instantiate(obstaclePrefab, pos, Quaternion.identity);
        }
    }
}
