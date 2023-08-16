using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer leftRenderer, rightRenderer;

    void Update()
    {
        leftRenderer.positionCount = rightRenderer.positionCount;
        var temp = new Vector3[rightRenderer.positionCount];
        rightRenderer.GetPositions(temp);
        leftRenderer.SetPositions(temp);
    }
}
