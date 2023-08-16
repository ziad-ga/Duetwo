using UnityEngine;
using System.Linq;
public class ObstacleGenerator : MonoBehaviour
{

    private GameObject[] normalObstacles;
    private GameObject[] hardObstacles;
    private GameObject lastObstacleObj;
    private GameObject lastObstaclePrefab;

    private int baseHardChance = 10;

    void Awake()
    {
        normalObstacles = (GameObject[])Resources.LoadAll("Prefabs/Obstacles/Normal").Cast<GameObject>().ToArray();
        hardObstacles = (GameObject[])Resources.LoadAll("Prefabs/Obstacles/Hard").Cast<GameObject>().ToArray();
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
        int hardChance = (int)(baseHardChance * GameManager.GameSpeed);
        GameObject[] pool = Random.Range(0, 100) < 100 - hardChance ? normalObstacles : hardObstacles;
        int temp = Random.Range(0, pool.Length);

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
            Vector3 pos = new Vector3(obstaclePrefab.transform.position.x, lastObstacleObj.transform.position.y + padding);
            lastObstaclePrefab = obstaclePrefab;
            return Instantiate(obstaclePrefab, pos, Quaternion.identity);
        }
    }
}
