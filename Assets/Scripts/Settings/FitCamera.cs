using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCamera : MonoBehaviour
{
    private float playableAreaRightBoundry;
    private float referenceSize;
    private readonly float referenceAspect = 9f / 20f;
    private void Awake()
    {
        referenceSize = Camera.main.orthographicSize;
        playableAreaRightBoundry = referenceSize * referenceAspect;
    }

    void Update()
    {
        Fit();
    }

    private void Fit()
    {
        var rightBoundry = Utility.GetScreenLeftRightBoundries().y;
        Camera.main.orthographicSize = Mathf.Max(referenceSize, playableAreaRightBoundry * Screen.height / Screen.width);
    }
}
