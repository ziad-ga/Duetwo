using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonsManager : MonoBehaviour
{
    private GameObject PauseButton;
    private GameObject PlayButton;
    private GameObject HomeButton;
    private float _pauseButtonYpos, _playButtonXpos, _homeButtonXpos;
    void Start()
    {
        PauseButton = transform.Find("PauseButton").gameObject;
        PlayButton = transform.Find("PlayButton").gameObject;
        HomeButton = transform.Find("HomeButton").gameObject;
        _pauseButtonYpos = PauseButton.transform.position.y;
        _playButtonXpos = PlayButton.transform.position.x;
        _homeButtonXpos = HomeButton.transform.position.x;
    }

    public void Pause()
    {
        GameManager.PauseGame();

        PauseButton.transform.parent.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0.5f), 0.3f).SetUpdate(true);
        PauseButton.transform.DOMoveY(6, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        PlayButton.transform.DOMoveX(1f, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        HomeButton.transform.DOMoveX(-1f, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
    }
    public void Play()
    {
        GameManager.ResumeGame();

        PauseButton.transform.parent.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.3f).SetUpdate(true);
        PauseButton.transform.DOMoveY(_pauseButtonYpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        PlayButton.transform.DOMoveX(_playButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
        HomeButton.transform.DOMoveX(_homeButtonXpos, 0.3f).SetEase(Ease.Linear).SetUpdate(true);
    }
}
