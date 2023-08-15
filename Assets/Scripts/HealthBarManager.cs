using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    private float widthPortion, heightPortion;
    private List<Vector3> points = new List<Vector3>();
    private float hpBottomMin, hpBottomMax, hpTopMin, hpTopMax, hpSideMin, hpSideMax;
    private float pointBottomMin, pointBottomMax, pointTopMin, pointTopMax, pointSideMin, pointSideMax;
    private Vector3 cameraWorldZero;

    private void Update()
    {
        FitPoints();
        // Bottom of the screen
        if (GameManager.HP < widthPortion * 100)
        {
            float x = Map(GameManager.HP / 100, hpBottomMin, hpBottomMax, pointBottomMin, pointBottomMax);
            points[1] = new Vector3(x, cameraWorldZero.y);
            lineRenderer.SetPositions(points.ToArray());
        }
        // Side of the screen
        else if (GameManager.HP >= widthPortion * 100 && GameManager.HP < (widthPortion + heightPortion) * 100)
        {
            lineRenderer.positionCount = 3;

            points[1] = new Vector3(pointBottomMax, cameraWorldZero.y);

            float y = Map(GameManager.HP / 100, hpSideMin, hpSideMax, pointSideMin, pointSideMax);
            points.Add(new Vector3(pointBottomMax, y));
            lineRenderer.SetPositions(points.ToArray());
        }
        // Top of the screen
        else if (GameManager.HP >= (widthPortion + heightPortion) * 100)
        {
            lineRenderer.positionCount = 4;

            points[1] = new Vector3(pointBottomMax, cameraWorldZero.y);
            points.Add(new Vector3(pointBottomMax, pointSideMax));

            float x = Map(GameManager.HP / 100, hpTopMin, hpTopMax, pointTopMin, pointTopMax);
            points.Add(new Vector3(x, pointSideMax));
            lineRenderer.SetPositions(points.ToArray());
        }
    }
    private void FitPoints()
    {
        cameraWorldZero = Utility.GetScreenZeroWorldPoint();
        // used to calculate hp boundaries
        heightPortion = (float)Screen.height / (Screen.height + Screen.width);
        widthPortion = Screen.width * 0.5f / (Screen.height + Screen.width);

        hpBottomMin = 0;
        hpBottomMax = widthPortion;
        pointBottomMin = 0;
        pointBottomMax = Utility.GetScreenLeftRightBoundries().y;


        hpSideMin = widthPortion;
        hpSideMax = widthPortion + heightPortion;
        pointSideMin = cameraWorldZero.y;
        pointSideMax = Utility.GetScreenTopBottomBoundries().x;


        hpTopMin = widthPortion + heightPortion;
        hpTopMax = 1;
        pointTopMin = pointBottomMax;
        pointTopMax = pointBottomMin;

        points.Clear();
        points.Add(new Vector3(0, cameraWorldZero.y));
        points.Add(new Vector3(0, cameraWorldZero.y));

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points.ToArray());
    }
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
}
