using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static Text scoreText;
    void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();
    }

}
