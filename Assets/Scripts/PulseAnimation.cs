using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PulseAnimation : MonoBehaviour
{   
    [SerializeField]
    private float scaleFactor = 1.05f;
    private float animationTime = 0.5f;

    void Start()
    {
        transform.DOScale(transform.localScale * scaleFactor, animationTime).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

}
