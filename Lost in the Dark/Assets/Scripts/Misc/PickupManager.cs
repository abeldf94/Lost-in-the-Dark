using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {

    // For health changes
    public PlayerDamageIndicator health;

    // For piston damage changes
    public Weapon pistol;

    // For speed move
    public PlayerMove move;

    // Avoid same powerup activation
    List<string> banned;

    // Constants
    private readonly float DELAY = 20;
    private readonly float SPEED = 7f;
    private readonly float MULTIPLIER = 10.0f;

    private void Start() {
        banned = new List<string>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Dynamite") {
            GameController.KillMonsters();
            Destroy(other.gameObject);
            Data.Items++;
            SoundManager.PlayBoom();
        } else if (other.gameObject.tag == "Gun") {
            if (!banned.Contains(other.gameObject.tag)) {
                StartCoroutine(IncreaseDamage());
                banned.Add(other.gameObject.tag);
            }
            Destroy(other.gameObject);
            Data.Items++;
        } else if (other.gameObject.tag == "Speed") {
            if (!banned.Contains(other.gameObject.tag)) {
                StartCoroutine(IncreaseSpeed());
                banned.Add(other.gameObject.tag);
            }
            Destroy(other.gameObject);
            Data.Items++;
        } else if (other.gameObject.tag == "HealthPack") {
            health.ResetHealth();
            Destroy(other.gameObject);
            Data.Items++;
        }

    }

    /// <summary>
    /// Increase pistol damage during x seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseDamage() {
        float temp = pistol.powerMultiplier;
        pistol.powerMultiplier = MULTIPLIER;
        yield return new WaitForSeconds(DELAY);
        pistol.powerMultiplier = temp;
    }

    /// <summary>
    /// Increase player speed during x seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseSpeed() {
        float temp = move.GetSpeed();
        move.SetSpeed(SPEED);
        yield return new WaitForSeconds(DELAY);
        move.SetSpeed(temp);
    }

}
