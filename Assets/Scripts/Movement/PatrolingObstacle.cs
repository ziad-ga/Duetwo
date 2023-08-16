using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PatrolingObstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(-1.25f, 1).SetDelay(1)).Append(transform.DOMoveX(1.25f, 1)).SetLoops(-1, LoopType.Restart);
    }

}
