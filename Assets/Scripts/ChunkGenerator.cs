using UnityEngine;
using System.Linq;
public class ChunkGenerator : MonoBehaviour
{

    private GameObject[] chunks;
    private  int currChunk = 0;
    private GameObject chunkObj;

    void Awake()
    {
        chunks = (GameObject[])Resources.LoadAll("Prefabs/Chunks").Cast<GameObject>().ToArray();
    }

    private void Start()
    {
        chunkObj = GetNextChunk();
    }

    private void Update()
    {
        if (chunkObj == null || GameManager.LastChildAppeared)
        {
            GameManager.LastChildAppeared = false;
            chunkObj = GetNextChunk();
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
            temp = Random.Range(0, chunks.Length);

        } while (temp == currChunk);

        currChunk = temp;

        GameObject ChunkPrefab = chunks[currChunk];

        // GameObject ChunkPrefab = (GameObject)Resources.Load("Prefabs/Chunks/Chunk1");

        Vector3 pos = new Vector3(ChunkPrefab.transform.position.x, ChunkPrefab.transform.position.y + GameManager.LastChildYpos);
        return Instantiate(ChunkPrefab, pos, Quaternion.identity);
    }
}
