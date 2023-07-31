using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{

    private int chunkCount;

    void Awake()
    {

        chunkCount = Resources.LoadAll("Prefabs/Chunks").Length;
    }

    private void Start()
    {
        GameManager.chunkObj = GetNextChunk();
    }

    private void Update()
    {
        if (GameManager.chunkObj == null || GameManager.lastChildAppeared)
        {
            GameManager.lastChildAppeared = false;
            GameManager.chunkObj = GetNextChunk();
        }
    }


    /// <summary>
    /// Returns a random chunk from the available chunks.
    /// </summary>
    private GameObject GetNextChunk()
    {
        int temp;
        do
        {
            temp = Random.Range(1, chunkCount + 1);

        } while (temp == GameManager.currChunk);

        GameManager.currChunk = temp;

        GameObject ChunkPrefab = (GameObject)Resources.Load("Prefabs/Chunks/Chunk" + GameManager.currChunk);

        // GameObject ChunkPrefab = (GameObject)Resources.Load("Prefabs/Chunks/Chunk1");

        Vector3 pos = new Vector3(ChunkPrefab.transform.position.x, ChunkPrefab.transform.position.y + GameManager.lastChildYpos, ChunkPrefab.transform.position.z);
        return Instantiate(ChunkPrefab, pos, Quaternion.identity);
    }
}
