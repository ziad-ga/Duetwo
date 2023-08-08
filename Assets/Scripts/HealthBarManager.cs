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
    private void Awake()
    {
        cameraWorldZero = Camera.main.ScreenToWorldPoint(Vector3.zero);
        // used to calculate hp boundaries
        heightPortion = (float)Screen.height / (Screen.height + Screen.width);
        widthPortion = Screen.width * 0.5f / (Screen.height + Screen.width);

        hpBottomMin = 0;
        hpBottomMax = widthPortion;
        pointBottomMin = 0;
        pointBottomMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;


        hpSideMin = widthPortion;
        hpSideMax = widthPortion + heightPortion;
        pointSideMin = cameraWorldZero.y;
        pointSideMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;


        hpTopMin = widthPortion + heightPortion;
        hpTopMax = 1;
        pointTopMin = pointBottomMax;
        pointTopMax = pointBottomMin;


        points.Add(new Vector3(0, cameraWorldZero.y));
        points.Add(new Vector3(0, cameraWorldZero.y));

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points.ToArray());

    }
    private void Update()
    {
        if (GameManager.HP < widthPortion * 100)
        {
            if (points.Count > 2)
            {
                points.RemoveRange(2, points.Count - 2);
                lineRenderer.positionCount = 2;
            }
            float x = Map(GameManager.HP / 100, hpBottomMin, hpBottomMax, pointBottomMin, pointBottomMax);
            points[1] = new Vector3(x, cameraWorldZero.y);
            lineRenderer.SetPositions(points.ToArray());
        }
        else if (GameManager.HP >= widthPortion * 100 && GameManager.HP < (widthPortion + heightPortion) * 100)
        {
            if (points.Count > 3)
            {
                points.RemoveAt(3);
                lineRenderer.positionCount = 3;

            }
            if (points.Count == 2)
            {
                points.Add(points[1]);
                lineRenderer.positionCount = 3;
            }
            float y = Map(GameManager.HP / 100, hpSideMin, hpSideMax, pointSideMin, pointSideMax);
            points[2] = new Vector3(pointBottomMax, y);
            lineRenderer.SetPositions(points.ToArray());
        }
        else if (GameManager.HP >= (widthPortion + heightPortion) * 100 && GameManager.HP < 100)
        {
            if (points.Count == 3)
            {
                points.Add(points[2]);
                lineRenderer.positionCount = 4;
            }
            float x = Map(GameManager.HP / 100, hpTopMin, hpTopMax, pointTopMin, pointTopMax);
            points[3] = new Vector3(x, pointSideMax);
            lineRenderer.SetPositions(points.ToArray());
        }
    }
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
}
