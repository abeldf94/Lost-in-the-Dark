using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    // Spawn speed based on game difficulty
    private enum Speed {
        EASY = 5,
        MEDIUM = 4,
        HARD = 3
    }

    // Spawned monsters quantity each time
    private enum Quantity {
        EASY = 2,
        MEDIUM = 3,
        HARD = 4
    }

    // Singleton pattern
    private static SpawnController instance;

    // All the spawns in game
    private Transform[] spawns;

    // Number of monsters
    private int maxInGameMonsters;
    // Number of monsters currently in game
    private int currentSpawned;

    // Delay before start the spawn of monster after game starts
    private int startSpawnDelay;
    // Time between every spawn
    private int spawnDelay;

    // Monster container for clear hierarchy 
    private GameObject monstersContainer;

    // Monster prefabs
    public GameObject[] monsterPrefab;

    public GameObject player;

    private int quantity;

    // Constants
    private readonly string MONSTERS = "monsters";
    private readonly int EASY_ENEMIES = 15;
    private readonly int MEDIUM_ENEMIES = 20;
    private readonly int HARD_ENEMIES = 25; // Maybe lag?
    private readonly float[] EASY_PROB = { 0.5f, 0.95f }; // 50%, 45% and 5 %
    private readonly float[] MEDIUM_PROB = { 0.45f, 0.93f }; // 45%, 43% and 7 %
    private readonly float[] HARD_PROB = { 0.45f, 0.90f }; // 45%, 40% and 10 %

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this) // If it's not this, destroy it
            Destroy(gameObject);

        SetBasicSettings();
    }

    // Use this for initialization
    void Start () {
        List<Transform> temp = new List<Transform>();
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Floor"))
            temp.Add(i.transform);

        spawns = temp.ToArray();

        monstersContainer = new GameObject();

        InitContainer();

        // After 10 seconds, the game will spawn a monster every 5 seconds
        InvokeRepeating("Spawn", startSpawnDelay, spawnDelay);
    }
	
	/// <summary>
    /// If the monsters in game are under the maximun allowed, spawn one
    /// using probabilities and farthest spawner from player
    /// </summary>
	void Spawn () {
        if (currentSpawned < maxInGameMonsters) {
            float max = 0.0f;
            Transform spawn = null;
            foreach (Transform i in spawns) {
                float distance = Vector3.Distance(i.position, player.transform.position);
                if (max < distance) {
                    max = distance;
                    spawn = i;
                }
            }
            if (spawn == null) 
                spawn = spawns[0];

            if (spawn != null) {
                for (int i = 0; i < quantity; i++) {
                    Instantiate(monsterPrefab[SelectRandomMonster()], spawn.position + Vector3.up, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0), monstersContainer.transform);
                    currentSpawned++;
                }
            }
        }
    }

    /// <summary>
    /// Choose a random index for spawn a monster
    /// </summary>
    /// <returns> The index </returns>
    private int SelectRandomMonster() {
        float random = Random.Range(0f, 1f);

        if (Difficulty.EASY == Config.LevelDifficulty) {
            if (random < EASY_PROB[0]) { // The skeleton 
                return 0;
            } else if (random >= EASY_PROB[0] && random < EASY_PROB[1]) { // The spider
                return 1;
            } else { // The boss
                return 2;
            }
        } else if (Difficulty.MEDIUM == Config.LevelDifficulty) {
            if (random < MEDIUM_PROB[0]) {
                return 0;
            } else if (random >= MEDIUM_PROB[0] && random < MEDIUM_PROB[1]) { 
                return 1;
            } else { 
                return 2;
            }
        } else {
            if (random < HARD_PROB[0]) {
                return 0;
            } else if (random >= HARD_PROB[0] && random < HARD_PROB[1]) { 
                return 1;
            } else { 
                return 2;
            }
        }
    }

    /// <summary>
    /// Set some important settings
    /// </summary>
    private void SetBasicSettings() {
        startSpawnDelay = 5;
        if (Config.LevelDifficulty == Difficulty.EASY) {
            maxInGameMonsters = EASY_ENEMIES;
            spawnDelay = (int) Speed.EASY;
            quantity = (int) Quantity.EASY;
        } else if (Config.LevelDifficulty == Difficulty.MEDIUM) {
            maxInGameMonsters = MEDIUM_ENEMIES;
            spawnDelay = (int) Speed.MEDIUM;
            quantity = (int) Quantity.MEDIUM;
        } else if (Config.LevelDifficulty == Difficulty.HARD) {
            maxInGameMonsters = HARD_ENEMIES;
            spawnDelay = (int) Speed.HARD;
            quantity = (int) Quantity.HARD;
        }
    }

    /// <summary>
    /// Some basics for the container
    /// </summary>
    private void InitContainer() {
        monstersContainer.transform.position = new Vector3();
        monstersContainer.name = MONSTERS;
    }

    /// <summary>
    /// Control function to each monster death
    /// </summary>
    public void RemoveMonster() {
        currentSpawned--;
    }
}
