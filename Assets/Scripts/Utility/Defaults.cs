using UnityEngine;
using System.Collections.Generic;
public static class Defaults
{
    readonly public static float BALL_ROTATION_SPEED = 190;
    readonly public static float BALL_ROTATION_SPEED_MULTIPLIER = 0.9f;
    readonly public static float BALL_HEIGHT = -3;
    readonly public static float NORMAL_OBSTACLE_SPEED = 2;
    readonly public static float NORMAL_OBSTACLE_SPEED_MULTIPLIER = 1.2f;
    readonly public static float ROTATING_OBSTACLE_SPEED = 320;
    readonly public static float ROTATING_OBSTACLE_SPEED_MULTIPLIER = 0.7f;
    readonly public static float GAME_UPDATE_INTERVAL = 5;
    readonly public static float HIT_DAMAGE = 35;
    readonly public static float HP = 40;
    readonly public static Dictionary<ObstacleType,int> OBSTACLE_WEIGHTS = new Dictionary<ObstacleType, int>()
    {
        // Normal
        {ObstacleType.NORMAL_HORIZONTAL, 5},
        {ObstacleType.NORMAL_VERTICAL, 1},
        {ObstacleType.BIG, 1},

        // Hard
        {ObstacleType.ROTATING, 1},
        {ObstacleType.PATROLLING, 3}
    };

}