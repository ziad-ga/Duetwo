using UnityEngine;
using System.Linq;
public class ObstacleGenerator : MonoBehaviour
{

    private GameObject[] obstacles;
    private GameObject lastObstacleObj;
    private GameObject lastObstaclePrefab;


    void Awake()
    {
        obstacles = (GameObject[])Resources.LoadAll("Prefabs/Obstacles").Cast<GameObject>().ToArray();
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
        int temp = Random.Range(0, obstacles.Length);

        GameObject obstaclePrefab = obstacles[temp];

        // GameObject ChunkPrefab = (GameObject)Resources.Load("Prefabs/Chunks/Chunk1");
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
