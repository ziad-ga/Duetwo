using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
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
    private Button helpButton;
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
    private Button creditsButton;
    [SerializeField]
    private Button closeCreditsButton;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject inputOverlayPanel;
    [SerializeField]
    private GameObject helpPanel;
    [SerializeField]
    private GameObject creditsPanel;

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

    [SerializeField]
    private ScriptableRendererFeature _fullScreenEffect;
    [SerializeField]
    private Material _fullScreenMaterial;

    private int effectIntensity = Shader.PropertyToID("_FullScreenIntensity");
    private float fullIntensity = 0.1f;
    private float flashIntensity = 0.5f;


    private bool fullScreenEffectIsEnabled = false;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        _fullScreenEffect.SetActive(false);
        Score = 0;
        Highscore = PlayerPrefs.GetInt("Highscore", 0);

        pauseButtonYpos = pauseButton.transform.position.y;
        playButtonXpos = playButton.transform.position.x;
        homeButtonXpos = homeButton.transform.position.x;

        StartCoroutine(ScoreFullHealthAnimation());
    }
    void Update()
    {
        Score = GameManager.Score;

        if (GameManager.HP >= 100 && !fullScreenEffectIsEnabled)
        {
            fullScreenEffectIsEnabled = true;
            GameManager.Music.DOPitch(1.3f, 0.15f).SetUpdate(true).OnComplete(() =>
             {
                 GameManager.Music.DOPitch(0.95f, 0.2f).SetUpdate(true);
             });
            if (PlayerPrefs.GetInt("VFX", 1) == 1) EnableFullScreenEffect();
        }
        else if (GameManager.HP < 100 && fullScreenEffectIsEnabled)
        {
            fullScreenEffectIsEnabled = false;
            GameManager.Music.DOPitch(0.85f, 1).SetUpdate(true);

            if (PlayerPrefs.GetInt("VFX", 1) == 1) StartCoroutine(DisableFullScreenEffect());
        }
    }
    private IEnumerator ScoreFullHealthAnimation()
    {
        var orgScale = scoreText.transform.localScale;
        var scale = 1.1f;
        while (true)
        {
            yield return new WaitUntil(() => GameManager.HP >= 100);
            var seq = DOTween.Sequence();
            var test = scoreText.transform.DOScale(new Vector3(scale, scale, scale), 0.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            scoreText.DOBlendableColor(new Color(0.7667851f, 0.6179246f, 1), 0.5f).SetUpdate(true);
            yield return new WaitUntil(() => GameManager.HP < 100);
            scoreText.DOBlendableColor(new Color(1, 1, 1), 0.5f).SetUpdate(true);
            test.Kill();
            transform.DOScale(orgScale, 0.5f).SetUpdate(true);
        }
    }

    private void EnableFullScreenEffect()
    {

        _fullScreenMaterial.SetFloat(effectIntensity, 0);
        _fullScreenEffect.SetActive(true);
        StartCoroutine(FlashFSE());
    }
    private IEnumerator FlashFSE()
    {
        var duration = 0.125f;
        var t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _fullScreenMaterial.SetFloat(effectIntensity, Mathf.Lerp(0, flashIntensity, t / duration));
            yield return null;
        }
        StartCoroutine(FSE());
    }
    private IEnumerator FSE()
    {
        var duration = 0.225f;
        var t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _fullScreenMaterial.SetFloat(effectIntensity, Mathf.Lerp(flashIntensity, fullIntensity, t / duration));
            yield return null;
        }
    }

    private IEnumerator DisableFullScreenEffect()
    {
        var duration = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            _fullScreenMaterial.SetFloat(effectIntensity, Mathf.Lerp(fullIntensity, 0, t / duration));
            yield return null;
        }
        _fullScreenEffect.SetActive(false);
    }
    private void _Play()
    {
        GameManager.ResumeGame();

        Button rightButton = restartButton.IsActive() ? restartButton : playButton;

        pauseButton.gameObject.SetActive(true);
        helpButton.interactable = true;

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
        helpButton.interactable = false;

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
        instance.creditsButton.gameObject.SetActive(true);

        instance.title.DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.highScoreText.DOFade(1, 1.5f).SetEase(Ease.InQuad);

        instance.startGameButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.startGameButton.interactable = true;

        instance.settingsButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.settingsButton.interactable = true;

        instance.creditsButton.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InQuad);
        instance.creditsButton.interactable = true;
    }
    public static void DisableMenu()
    {
        instance.title.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.title.gameObject.SetActive(false);
        instance.highScoreText.DOFade(0, 0.5f).SetEase(Ease.InQuad).onComplete += () => instance.highScoreText.gameObject.SetActive(false);
        instance.startGameButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.startGameButton.gameObject.SetActive(false));
        instance.settingsButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.settingsButton.gameObject.SetActive(false));
        instance.creditsButton.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InQuad).OnComplete(() => instance.creditsButton.gameObject.SetActive(false));

        instance.scoreText.gameObject.SetActive(true);
        instance.pauseButton.gameObject.SetActive(true);
        instance.helpButton.gameObject.SetActive(true);
        instance.helpButton.interactable = true;

        instance.scoreText.DOFade(1, 0.5f);
        instance.pauseButton.GetComponent<Image>().DOFade(1, 0.5f);
        instance.helpButton.GetComponent<Image>().DOFade(1, 0.5f);
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
    public static void OpenHelpPrompt()
    {
        instance.OpenHelp();
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
        helpButton.GetComponent<Image>().DOFade(0, 0.3f).SetUpdate(true);

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
    public void OpenHelp()
    {
        GameManager.PauseGame();
        helpPanel.SetActive(true);
        helpPanel.GetComponent<Button>().interactable = true;
        helpButton.interactable = false;
        helpPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void CloseHelp()
    {
        helpPanel.GetComponent<Button>().interactable = false;
        helpPanel.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            GameManager.ResumeGame();
            helpPanel.SetActive(false);
            helpButton.interactable = true;
        });
    }
    public void StartGame()
    {
        startGameButton.interactable = false;
        GameManager.StartGame();
    }
    public void OpenCredits()
    {
        creditsButton.interactable = false;
        creditsPanel.SetActive(true);
        creditsPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void CloseCredits()
    {
        creditsPanel.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            creditsPanel.SetActive(false);
            creditsButton.interactable = true;
        });
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
