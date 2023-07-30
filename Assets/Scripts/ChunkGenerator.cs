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
        if (GameManager.chunkObj.transform.childCount == 0)
        {
            Destroy(GameManager.chunkObj);
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
        return Instantiate(ChunkPrefab, ChunkPrefab.transform.position, Quaternion.identity);

        // GameObject ChunkPrefab = (GameObject)Resources.Load("Prefabs/Chunks/Chunk2");
        // return Instantiate(ChunkPrefab, ChunkPrefab.transform.position, Quaternion.identity);
    }
}
