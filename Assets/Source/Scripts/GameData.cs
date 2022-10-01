using UnityEngine;

public static class GameData
{
    public static EnemySpawn _enemySpawn
    {
        get
        {
            if (__enemySpawn == null) __enemySpawn = GameObject.FindObjectOfType<EnemySpawn>();

            return __enemySpawn;
        }
        set
        {
            __enemySpawn = value;
        }
    }

    private static EnemySpawn __enemySpawn;

    public static GameSystem _gameSystem
    {
        get
        {
            if (__gameSystem == null) __gameSystem = GameObject.FindObjectOfType<GameSystem>();

            return __gameSystem;
        }
        set
        {
            __gameSystem = value;
        }
    }

    private static GameSystem __gameSystem;
    public static CanvasUI _canvasUI
    {
        get
        {
            if (__canvasUI == null) __canvasUI = GameObject.FindObjectOfType<CanvasUI>();

            return __canvasUI;
        }
        set
        {
            __canvasUI = value;
        }
    }

    private static CanvasUI __canvasUI;
}