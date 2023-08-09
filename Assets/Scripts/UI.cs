using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static UI instance;
    private Text scoreText, title;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button startGameButton;
    private float Score { get { return int.Parse(scoreText.text); } set { scoreText.text = ((int)value).ToString(); } }

    private float pauseButtonYpos, playButtonXpos, homeButtonXpos;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        scoreText = transform.Find("Score").GetComponent<Text>();
        title = transform.Find("Title").GetComponent<Text>();
        Score = 0;

        // _scoreText.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.5f).SetUpdate(true);

        pauseButtonYpos = pauseButton.transform.position.y;
        playButtonXpos = playButton.transform.position.x;
        homeButtonXpos = homeButton.transform.position.x;
    }
    void Update()
    {
        Score = GameManager.Score;
    }

    public static void EnableMenu()
    {
        instance.title.gameObject.SetActive(true);
        instance.startGameButton.gameObject.SetActive(true);
        instance.title.DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.startGameButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.startGameButton.interactable = true;
    }
    public static void DisableMenu()
    {
        instance.title.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.title.gameObject.SetActive(false);
        instance.startGameButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.startGameButton.gameObject.SetActive(false));

        instance.scoreText.gameObject.SetActive(true);
        instance.scoreText.DOFade(1, 1).SetUpdate(true);

        instance.pauseButton.gameObject.SetActive(true);
        instance.pauseButton.transform.DOMoveY(instance.pauseButtonYpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true);

    }
    #region Buttons
    public void Pause()
    {
        GameManager.PauseGame();
        playButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0.5f, 0.3f).SetUpdate(true);
        pauseButton.transform.DOMoveY(6, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => pauseButton.gameObject.SetActive(false));

        playButton.transform.DOMoveX(1f, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        homeButton.transform.DOMoveX(-1f, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
    }
    public void Play()
    {
        GameManager.ResumeGame();

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true); // Fade out the panel

        pauseButton.gameObject.SetActive(true);
        pauseButton.transform.DOMoveY(pauseButtonYpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true);

        playButton.transform.DOMoveX(playButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => { playButton.gameObject.SetActive(false); });
        homeButton.transform.DOMoveX(homeButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
    }
    public void Home()
    {
        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true);
        playButton.transform.DOMoveX(playButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => { playButton.gameObject.SetActive(false); });
        homeButton.transform.DOMoveX(homeButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
        scoreText.DOFade(0, 1).SetUpdate(true).OnComplete(() => { scoreText.gameObject.SetActive(false); });

        GameManager.TransitionToMenu();
    }
    public void StartGame()
    {
        startGameButton.interactable = false;
        GameManager.StartGame();
    }
    #endregion
}
