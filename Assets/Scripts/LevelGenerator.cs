using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private int levelCount;

    void Awake()
    {

        levelCount = Resources.LoadAll("Prefabs/Levels").Length;
    }

    private void Update()
    {
        if (GameManager.levelObj.transform.childCount == 0)
        {
            Destroy(GameManager.levelObj);
            GameManager.levelObj = GetNextLevel();
        }
    }

    private GameObject GetNextLevel()
    {
        int temp;
        do
        {
            temp = Random.Range(1, levelCount + 1);

        } while (temp == GameManager.currLevel);

        GameManager.currLevel = temp;

        GameObject levelPrefab = (GameObject)Resources.Load("Prefabs/Levels/Level" + GameManager.currLevel);
        return Instantiate(levelPrefab, levelPrefab.transform.position, Quaternion.identity);

        // GameObject levelPrefab = (GameObject)Resources.Load("Prefabs/Levels/Level2");
        // return Instantiate(levelPrefab, levelPrefab.transform.position, Quaternion.identity);
    }
}
