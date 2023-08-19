using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BackgroundRandomAnimation : MonoBehaviour
{
    private float animationDelay;
    private float animationDirection;
    void Start()
    {
        animationDelay = Random.Range(1, 4);
        animationDirection = Random.Range(0, 2) == 0 ? 1 : -1;

        StartCoroutine(Animate());
        StartCoroutine(FullHealthAnimation());
    }

    private IEnumerator FullHealthAnimation()
    {
        var orgScale = transform.localScale;
        while (true)
        {
            yield return new WaitUntil(() => GameManager.HP >= 100);
            var scale = Random.Range(1, 1.3f);
            var time = Random.Range(0.5f, 0.75f);
            var test = transform.DOScale(new Vector3(scale, scale, scale), time).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            yield return new WaitUntil(() => GameManager.HP < 100);
            test.Kill();
            transform.DOScale(orgScale, 0.5f).SetUpdate(true);
        }
    }
    private IEnumerator Animate()
    {
        yield return new WaitForSecondsRealtime(animationDelay);

        while (true)
        {
            transform.DORotate(new Vector3(0, 0, 60 * animationDirection), 0.5f, RotateMode.LocalAxisAdd).SetUpdate(true);

            yield return new WaitForSecondsRealtime(0.5f + animationDelay);
        }
    }
}
