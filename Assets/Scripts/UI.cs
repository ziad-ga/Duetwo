using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static UI instance;
    private Text _scoreText;
    [SerializeField]
    private GameObject _healthBar;
    public static float Score { get { return Score; } set { instance._scoreText.text = ((int)value).ToString(); } }
    public static float HP { get { return HP; } set { instance._healthBar.transform.localScale = new Vector3(value, instance._healthBar.transform.localScale.y, 1); } }
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
        _scoreText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f).SetUpdate(true);
        HP = Defaults.HP / 100;
        _healthBar.SetActive(true);
    }
    void Update()
    {
        HP = GameManager.HP / 100;
        Score = GameManager.Score;
    }

}
