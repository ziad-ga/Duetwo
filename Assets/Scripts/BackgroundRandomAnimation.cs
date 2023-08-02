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
    }
    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(animationDelay);

        while (true)
        {
            transform.DORotate(new Vector3(0, 0, 60 * animationDirection), 0.5f, RotateMode.LocalAxisAdd);
            
            yield return new WaitForSeconds(0.5f + animationDelay);
        }
    }
}
