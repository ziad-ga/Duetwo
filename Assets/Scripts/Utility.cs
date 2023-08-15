using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    /// <summary>
    /// Returns a Vector2(left boundry, right boundry) with the left and right boundaries of the screen in world space.
    /// </summary>
    public static Vector2 ScreenLeftRightBoundries(){
        return new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x);
    }
    /// <summary>
    /// Returns a Vector2(top boundry, bottom boundry) with the top and bottom boundaries of the screen in world space.
    /// </summary>
    public static Vector2 ScreenTopBottomBoundries(){
        return new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y, Camera.main.ScreenToWorldPoint(Vector3.zero).y);
    }
    /// <summary>
    /// Returns Screen zero (bottom left corner) point in world space.
    /// </summary>
    public static Vector3 ScreenZeroWorldPoint(){
        return Camera.main.ViewportToWorldPoint(Vector3.zero);
    }

}
