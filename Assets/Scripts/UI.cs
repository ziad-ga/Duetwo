using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static Text scoreText;
    void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<Text>();
        scoreText.text = "0";
        scoreText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f);
    }

}
