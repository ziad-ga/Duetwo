using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class UI : MonoBehaviour
{
    public static UI instance;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject settingsPanel;
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

        Score = 0;

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
        instance.settingsButton.gameObject.SetActive(true);

        instance.title.DOFade(1, 1.5f).SetEase(Ease.InQuad);

        instance.startGameButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.startGameButton.interactable = true;

        instance.settingsButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.settingsButton.interactable = true;
    }
    public static void DisableMenu()
    {
        instance.title.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.title.gameObject.SetActive(false);
        instance.startGameButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.startGameButton.gameObject.SetActive(false));
        instance.settingsButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.settingsButton.gameObject.SetActive(false));

        instance.scoreText.gameObject.SetActive(true);
        instance.pauseButton.gameObject.SetActive(true);

        instance.scoreText.DOFade(1, 1);
        instance.pauseButton.GetComponent<Image>().DOFade(1, 1);

    }
    #region Buttons
    public void Pause()
    {
        GameManager.PauseGame();
        playButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0.5f, 0.3f).SetUpdate(true);
        pauseButton.transform.DOBlendableMoveBy(new Vector3(0, 0.1f * Screen.height), 0.3f).SetUpdate(true).OnComplete(() => pauseButton.gameObject.SetActive(false));

        playButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true);
        homeButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true);
    }
    public void Play()
    {
        GameManager.ResumeGame();
        pauseButton.gameObject.SetActive(true);

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true); // Fade out the panel

        pauseButton.transform.DOBlendableMoveBy(new Vector3(0, -0.1f * Screen.height, 0), 0.3f).SetEase(Ease.Linear).SetUpdate(true);

        playButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { playButton.gameObject.SetActive(false); });
        homeButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
    }
    public void Home()
    {
        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true); // Fade out the panel

        pauseButton.transform.position = new Vector3(pauseButton.transform.position.x, pauseButton.transform.position.y - 0.1f * Screen.height);
        pauseButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        playButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { playButton.gameObject.SetActive(false); });
        homeButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
        scoreText.DOFade(0, 1).SetUpdate(true).OnComplete(() => { scoreText.gameObject.SetActive(false); });

        GameManager.TransitionToMenu();
    }
    public void StartGame()
    {
        startGameButton.interactable = false;
        GameManager.StartGame();
    }
    public void Settings()
    {
        settingsButton.interactable = false;
        startGameButton.interactable = false;
        mainPanel.GetComponent<Button>().interactable = true;

        Camera.main.transform.DOBlendableMoveBy(new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, 0), 0.5f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
        settingsPanel.transform.DOBlendableMoveBy(new Vector3(0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);

    }
    public void CloseSettings()
    {
        settingsButton.interactable = true;
        startGameButton.interactable = true;
        mainPanel.GetComponent<Button>().interactable = false;

        Camera.main.transform.DOMoveX(0, 0.5f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(-0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
        settingsPanel.transform.DOBlendableMoveBy(new Vector3(-0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
    }
    #endregion
}
