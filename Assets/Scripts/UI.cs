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
    [SerializeField]
    private GameObject inputOverlayPanel;
    [SerializeField]
    private GameObject[] SettingsItems;
    [SerializeField]
    private GameObject currSettingsItem, nextSettingsItem;
    private int settingsIdx = 0;
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
    public void OpenSettings()
    {
        inputOverlayPanel.SetActive(true);

        Camera.main.transform.DOBlendableMoveBy(new Vector3(Camera.main.ScreenToWorldPoint(Vector3.zero).x, 0), 0.5f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
        settingsPanel.transform.DOBlendableMoveBy(new Vector3(0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);

    }
    public void CloseSettings()
    {
        inputOverlayPanel.SetActive(false);

        Camera.main.transform.DOMoveX(0, 0.5f).SetEase(Ease.InOutQuad);
        mainPanel.transform.DOBlendableMoveBy(new Vector3(-0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
        settingsPanel.transform.DOBlendableMoveBy(new Vector3(-0.5f * Screen.width, 0), 0.5f).SetEase(Ease.InOutQuad);
    }
    public void GetNext()
    {
        float transTime = 0.25f;

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
