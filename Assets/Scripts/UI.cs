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
    private Text highScoreText;
    [SerializeField]
    private Text restartScoreText;
    [SerializeField]
    private Text restartHighscoreText;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button restartButton;
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
    [SerializeField]
    private GameObject inputOverlayPanel;
    [SerializeField]
    private GameObject[] SettingsItems;
    [SerializeField]
    private GameObject currSettingsItem, nextSettingsItem;
    private int settingsIdx = 0;
    private float Score { set { scoreText.text = ((int)value).ToString(); } }
    private int Highscore { set { highScoreText.text = $"HIGHSCORE\n{value}"; } }
    private float RestartScore { set { restartScoreText.text = $"SCORE\n{(int)value}"; } }
    private int RestartHighscore { set { restartHighscoreText.text = $"HIGHSCORE\n{value}"; } }



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
        Highscore = PlayerPrefs.GetInt("Highscore", 0);

        pauseButtonYpos = pauseButton.transform.position.y;
        playButtonXpos = playButton.transform.position.x;
        homeButtonXpos = homeButton.transform.position.x;
    }
    void Update()
    {
        Score = GameManager.Score;
    }

    private void _Play()
    {
        GameManager.ResumeGame();

        Button rightButton = restartButton.IsActive() ? restartButton : playButton;

        pauseButton.gameObject.SetActive(true);

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true); // fade out the panel

        pauseButton.transform.DOBlendableMoveBy(new Vector3(0, -0.1f * Screen.height, 0), 0.3f).SetEase(Ease.Linear).SetUpdate(true);

        rightButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { rightButton.gameObject.SetActive(false); });
        homeButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
    }
    private void _Pause()
    {
        GameManager.PauseGame();

        Button rightButton = restartButton.IsActive() ? restartButton : playButton;

        rightButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);

        pauseButton.transform.parent.GetComponent<Image>().DOFade(0.5f, 0.3f).SetUpdate(true); // fade in the panel
        pauseButton.transform.DOBlendableMoveBy(new Vector3(0, 0.1f * Screen.height), 0.3f).SetUpdate(true).OnComplete(() => pauseButton.gameObject.SetActive(false));

        rightButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true);
        homeButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true);
    }
    public static void EnableMenu()
    {
        instance.Highscore = PlayerPrefs.GetInt("Highscore", 0);

        instance.title.gameObject.SetActive(true);
        instance.highScoreText.gameObject.SetActive(true);
        instance.startGameButton.gameObject.SetActive(true);
        instance.settingsButton.gameObject.SetActive(true);

        instance.title.DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.highScoreText.DOFade(1, 1.5f).SetEase(Ease.InQuad);

        instance.startGameButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.startGameButton.interactable = true;

        instance.settingsButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.settingsButton.interactable = true;
    }
    public static void DisableMenu()
    {
        instance.title.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.title.gameObject.SetActive(false);
        instance.highScoreText.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.highScoreText.gameObject.SetActive(false);
        instance.startGameButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.startGameButton.gameObject.SetActive(false));
        instance.settingsButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.settingsButton.gameObject.SetActive(false));

        instance.scoreText.gameObject.SetActive(true);
        instance.pauseButton.gameObject.SetActive(true);

        instance.scoreText.DOFade(1, 0.5f);
        instance.pauseButton.GetComponent<Image>().DOFade(1, 0.5f);
    }
    public static void EnableRestartPrompt()
    {
        instance.restartButton.gameObject.SetActive(true);

        instance._Pause();

        instance.RestartScore = GameManager.Score;
        instance.RestartHighscore = PlayerPrefs.GetInt("Highscore", 0);
        instance.restartScoreText.gameObject.SetActive(true);
        instance.restartHighscoreText.gameObject.SetActive(true);

        instance.restartScoreText.DOFade(1, 0.5f).SetEase(Ease.InQuad).SetUpdate(true);
        instance.restartHighscoreText.DOFade(1, 0.5f).SetEase(Ease.InQuad).SetUpdate(true);
    }



    #region Buttons
    public void Pause()
    {
        _Pause();
    }
    public void Play()
    {
        _Play();
    }
    public void Restart()
    {
        _Play();
        GameManager.RestartGame();

        instance.restartScoreText.DOFade(0, 0.5f).SetEase(Ease.InQuad).SetUpdate(true).onComplete += () => instance.restartScoreText.gameObject.SetActive(false);
        instance.restartHighscoreText.DOFade(0, 0.5f).SetEase(Ease.InQuad).SetUpdate(true).onComplete += () => instance.restartHighscoreText.gameObject.SetActive(false);
    }
    public void Home()
    {
        pauseButton.transform.parent.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true); // Fade out the panel

        pauseButton.transform.position = new Vector3(pauseButton.transform.position.x, pauseButton.transform.position.y - 0.1f * Screen.height);
        pauseButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        if (restartButton.IsActive())
        {
            restartButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { restartButton.gameObject.SetActive(false); });

            restartScoreText.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => { restartScoreText.gameObject.SetActive(false); });
            restartHighscoreText.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => { restartHighscoreText.gameObject.SetActive(false); });

        }
        else
            playButton.transform.DOBlendableMoveBy(new Vector3(0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { playButton.gameObject.SetActive(false); });

        homeButton.transform.DOBlendableMoveBy(new Vector3(-0.4f * Screen.width, 0), 0.3f).SetUpdate(true).OnComplete(() => { homeButton.gameObject.SetActive(false); });
        scoreText.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => { scoreText.gameObject.SetActive(false); });

        GameManager.TransitionToMenu();
    }
    public void StartGame()
    {
        startGameButton.interactable = false;
        GameManager.StartGame();
    }
    public void OpenSettings()
    {
        inputOverlayPanel.SetActive(true);

        Camera.main.transform.DOBlendableMoveBy(new Vector3(Utility.GetScreenLeftRightBoundries().x, 0), 0.25f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(0.5f * Screen.width, 0), 0.25f).SetEase(Ease.InOutQuad);
        settingsPanel.GetComponent<RectTransform>().DOMoveX(0, 0.25f).SetEase(Ease.InOutQuad);


    }
    public void CloseSettings()
    {
        inputOverlayPanel.SetActive(false);

        Camera.main.transform.DOMoveX(0, 0.25f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(-0.5f * Screen.width, 0), 0.25f).SetEase(Ease.InOutQuad);
        settingsPanel.transform.DOMoveX(-Screen.width, 0.25f).SetEase(Ease.InOutQuad);

    }
    public void GetNext()
    {
        float transTime = 0.1f;

        nextSettingsItem.GetComponent<Button>().interactable = false;


        foreach (var toggle in SettingsItems[settingsIdx].GetComponentsInChildren<Toggle>())
        {
            foreach (var image in toggle.gameObject.GetComponentsInChildren<Image>())
            {
                image.DOFade(0, transTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            }
            toggle.gameObject.GetComponentInChildren<Text>().DOFade(0, transTime).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
            {
                toggle.gameObject.SetActive(false);
            });
        }
        settingsIdx = (settingsIdx + 1) % SettingsItems.Length;

        foreach (var toggle in SettingsItems[settingsIdx].GetComponentsInChildren<Toggle>(includeInactive: true))
        {
            toggle.gameObject.SetActive(true);
            foreach (var image in toggle.gameObject.GetComponentsInChildren<Image>(includeInactive: true))
            {
                image.DOFade(1, transTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            }
            toggle.gameObject.GetComponentInChildren<Text>(includeInactive: true).DOFade(1, transTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        }

        var nextIdx = (settingsIdx + 1) % SettingsItems.Length;
        var tempPos = nextSettingsItem.transform.position.x;

        nextSettingsItem.transform.DOMoveX(currSettingsItem.transform.position.x, transTime).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
        {
            var tempItem = nextSettingsItem;
            nextSettingsItem = currSettingsItem;
            currSettingsItem = tempItem;

            nextSettingsItem.GetComponent<Text>().DOFade(0, 0).SetUpdate(true);
            nextSettingsItem.transform.DOMoveX(tempPos, 0).SetUpdate(true);
            nextSettingsItem.GetComponent<Text>().text = SettingsItems[nextIdx].name;
            nextSettingsItem.GetComponent<Text>().DOFade(1, transTime).SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
            {
                nextSettingsItem.GetComponent<Button>().interactable = true;
            });
        });
        currSettingsItem.transform.DOMoveX(currSettingsItem.transform.position.x - currSettingsItem.GetComponent<RectTransform>().rect.width, transTime).SetEase(Ease.InOutQuad).SetUpdate(true);

    }
    public void HandleToggle(Toggle toggle)
    {
        Options.HandlePlayerSettings(toggle.name, toggle.isOn);
    }
    #endregion
}
