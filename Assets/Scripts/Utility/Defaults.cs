using UnityEngine;
using System.Collections.Generic;
public static class Defaults
{
    readonly public static float BALL_ROTATION_SPEED = 190;
    readonly public static float BALL_ROTATION_SPEED_MULTIPLIER = 0.9f;
    readonly public static float BALL_HEIGHT = -3;
    readonly public static float NORMAL_OBSTACLE_SPEED = 1.5f;
    readonly public static float NORMAL_OBSTACLE_SPEED_MULTIPLIER = 1.3f;
    readonly public static float ROTATING_OBSTACLE_SPEED = 320;
    readonly public static float ROTATING_OBSTACLE_SPEED_MULTIPLIER = 0.9f;
    readonly public static float GAME_UPDATE_INTERVAL = 5;
    readonly public static float GAME_MIN_SPEED = 1f;
    readonly public static float GAME_MAX_SPEED = 10;
    readonly public static float GAME_COLLISION_SPEED_PENALITY = 0.75f;
    readonly public static float FULL_HP_SPEED_BONUS = 0.4f;

    readonly public static float HP = 40;
    readonly public static float HIT_DAMAGE = 45;
    readonly public static float HP_PER_SECOND = 5;
    readonly public static Dictionary<ObstacleType, int> OBSTACLE_WEIGHTS = new Dictionary<ObstacleType, int>()
    {
        // Normal
        {ObstacleType.NORMAL_HORIZONTAL, 5},
        {ObstacleType.NORMAL_VERTICAL, 1},
        {ObstacleType.BIG, 1},

        // Hard
        {ObstacleType.ROTATING, 0},
        {ObstacleType.PATROLLING, 4}
    };

}