using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static UI instance;
    private Text _scoreText;
    public static Text ScoreText { get { return instance._scoreText; } }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        _scoreText = transform.Find("Score").GetComponent<Text>();
        _scoreText.text = "0";
        _scoreText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f);
    }

}
