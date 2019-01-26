using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {

    // The player targer for navmesh
    private GameObject player;

    // The agent that control the monster
    private NavMeshAgent agent;

    // The position in last update
    private Vector3 lastPosition;

    // Monster health to know if he died
    private MonsterHealth health;

    // Constant limit distance  
    public static float LIMIT = 3f;

    // Minimun distance to recognize if player change position
    private readonly float THRESHOLD = 0.1f;

    // The rotation speed for face the player
    private readonly int ROTATION_SPEED = 5;

    // The powerUps
    public GameObject[] powerUps;

    // Monster status
    private bool stopped;

    // Default sound
    private AudioSource sound;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player for follow
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<MonsterHealth>();
        lastPosition = player.transform.position;
        stopped = false;
        GameController.KillAllMonsters += KillMonster;
        sound = GetComponent<AudioSource>();
        
        // Play his roar randomly every random time
        InvokeRepeating("PlayRoar", UnityEngine.Random.Range(30f, 60f), UnityEngine.Random.Range(30f, 60f));
    }

    void Update () {
        if (!health.IsDead())
            CheckPlayerPosition();
        else if (!stopped) {
            agent.enabled = false;
            stopped = true;
        }
    }

    /// <summary>
    /// Called when this monster dead
    /// </summary>
    public void MonsterDeathItem() {
        GameObject powerUp = DropRandomItem();
        if (powerUp != null)
            SpawnObject.Spawn(transform.position + Vector3.up, powerUp);
    }


    /// <summary>
    /// The monster has a 1% chance to drop
    /// If finally, it drop something, the probabilities
    /// of each item are:
    /// 50 % Health restore
    /// 30 % Speed increase for 20 seconds
    /// 15 % Damage increase for 20 seconds
    /// 5 % Dynamite (Kill all in game monsters)
    /// </summary>
    private GameObject DropRandomItem() {
        if (powerUps == null || powerUps.Length != 4)
            throw new UnityException("Ups, something was wrong with the power ups");

        float random = UnityEngine.Random.Range(0f, 1f);
        if (random <= 0.01) {
            random = UnityEngine.Random.Range(0f, 1f);
            if (random <= 0.5f) 
                return powerUps[3];
            else if (random > 0.5f && random <= 0.8f) 
                return powerUps[2];
            else if (random > 0.8f && random <= 0.95f) 
                return powerUps[1];
            else
                return powerUps[0];
        }

        return null;
    }

    private void CheckPlayerPosition() {
        // Just update if the distance between the player actual position and old is > than a limit 
        float offset = Vector3.Distance(player.transform.position, lastPosition);
        if (offset > THRESHOLD || offset < THRESHOLD) {
            lastPosition = player.transform.position;
            agent.SetDestination(player.transform.position);
        }       
    }

    // Event called at the end of attack animation to make him face the player
    private void LateUpdate() {
        if (agent.velocity == Vector3.zero) {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle > 20) {
                Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATION_SPEED * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Make the monster die
    /// </summary>
    private void KillMonster() {
        health.Damage(Int32.MaxValue);
    }

    /// <summary>
    /// Play his roar
    /// </summary>
    private void PlayRoar() {
        sound.Play();
    }
}
