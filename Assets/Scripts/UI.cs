using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static UI instance;
    private Text _scoreText;
    private float Score { get { return int.Parse(_scoreText.text); } set { _scoreText.text = ((int)value).ToString(); } }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        _scoreText = transform.Find("Score").GetComponent<Text>();
        Score = 0;
        _scoreText.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f).SetUpdate(true);
    }
    void Update()
    {
        Score = GameManager.Score;
    }

}
