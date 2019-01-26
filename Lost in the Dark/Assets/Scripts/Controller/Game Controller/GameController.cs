using Maze; // Namespace Maze
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Singleton pattern
	private static GameController instance;
    // Maze generation
    private MazeGenerator generator;

    public bool useCustomSeed = false;
    public int seed = 123321;

    // Maze size
    private int columns;
    private int rows;

    // Game objects
    public GameObject[] wallPrefab = null;
    public GameObject floorPrefab = null;
    public GameObject pillarPrefab = null;
    public GameObject[] doorArchPrefab = null;
    public GameObject ceilingPrefab = null;

    // Constants
    private readonly string SCENE = "End Menu";
    private readonly string THIS = "Game";
    private readonly int EASY_SIZE = 7;
    private readonly int MEDIUM_SIZE = 6;
    private readonly int HARD_SIZE = 5;
    
    // The player
    public GameObject player;

    // The spawn locantions depending on difficulty
    private readonly Vector3 EASY_POSITION = new Vector3(21.5f, 0.5f, 31.5f);
    private readonly Vector3 MEDIUM_POSITION = new Vector3(16.4f, 0.5f, 19.4f);
    private readonly Vector3 HARD_POSITION = new Vector3(10.2f, 0.5f, 13.4f);

    // Timer for know how many time the player was playing
    private Stopwatch timer;

    // Events 
    public delegate void PlayerDeath();
    public static event PlayerDeath PlayerDeathEvent;

    public delegate void MonsterKiller();
    public static event MonsterKiller KillAllMonsters;


    // Use this for initialization
    void Awake () {
        if (instance == null) 
			instance = this;
		else if (instance != this) // If it's not this, destroy it
			Destroy(gameObject);

        SetBasicSettings();

        PlayerDeathEvent = EndGame;

        CheckForPrefabs();

        generator = new MazeGenerator(columns, rows, useCustomSeed, seed, wallPrefab, doorArchPrefab, pillarPrefab);
        generator.BuildInUnity(floorPrefab, ceilingPrefab);

        CreateNavMeshForLevel();

        timer = new Stopwatch();
        timer.Start();
    }

    private void SetBasicSettings() {
        if (Config.LevelDifficulty == Difficulty.EASY) {
            rows = EASY_SIZE;
            columns = EASY_SIZE;
            player.transform.position = EASY_POSITION;
        } else if (Config.LevelDifficulty == Difficulty.MEDIUM) {
            rows = MEDIUM_SIZE;
            columns = MEDIUM_SIZE;
            player.transform.position = MEDIUM_POSITION;
        } else if(Config.LevelDifficulty == Difficulty.HARD) {
            rows = HARD_SIZE;
            columns = HARD_SIZE;
           player.transform.position = HARD_POSITION;
        }

    }

    /// <summary>
    /// Transition after player died
    /// </summary>
    private void EndGame() {
        timer.Stop();
        Data.Time = string.Format("{0:00}:{1:00}:{2:00}",
               (int)timer.Elapsed.TotalHours,
                    timer.Elapsed.Minutes,
                    timer.Elapsed.Seconds);
        Data.Bullets = FindObjectOfType<Weapon>().bullets;
        SceneManager.LoadScene(SCENE, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(THIS);
    }

    private void CheckForPrefabs() {
        if (wallPrefab == null)
            throw new UnityException("Choose prefabs before starting");
        else if (floorPrefab == null)
            throw new UnityException("Choose prefabs before starting");
        else if (pillarPrefab == null)
            throw new UnityException("Choose prefabs before starting");
        else if (doorArchPrefab == null)
            throw new UnityException("Choose prefabs before starting");
        else if (ceilingPrefab == null)
            throw new UnityException("Choose prefabs before starting");
    }

    /// <summary>
    /// Generate the surface for the nav agents in game. Remember each maze generation it's different to previous.
    /// </summary>
    private void CreateNavMeshForLevel() {
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        NavMeshSurface surface = level.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    /// <summary>
    /// Transition to end scene when player is dead
    /// </summary>
    public static void UpdateDeath() {
        if (PlayerDeathEvent != null) 
            PlayerDeathEvent();
    }

    /// <summary>
    /// Call the event for kill all the monsters
    /// </summary>
    public static void KillMonsters() {
        if (KillAllMonsters != null)
            KillAllMonsters();
    }
}
