using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {

    // The player damage indicator for add damage
    PlayerDamageIndicator player;

    // List of attacked monsters
    List<GameObject> monsters;

    // Delay that every monster cant attack
    private readonly float DELAY = 0.5f;

    private void Start() {
        monsters = new List<GameObject>();
        player = GetComponent<PlayerDamageIndicator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Sword" && !monsters.Contains(other.gameObject)) { // Damage for skeleton
            player.Damage(Random.Range(15, 25));
            StartCoroutine(Disable(other.gameObject));
        } else if (other.gameObject.tag == "Sting" && !monsters.Contains(other.gameObject)) { // Damage for spider
            player.Damage(Random.Range(10, 20));
            StartCoroutine(Disable(other.gameObject));
        } else if (other.gameObject.tag == "Mace" && !monsters.Contains(other.gameObject)) { // Damage for Troll
            player.Damage(Random.Range(50, 100));
            StartCoroutine(Disable(other.gameObject));
        }
    }

    // Prevent more than one hit in same attack
    IEnumerator Disable(GameObject monster) {
        monsters.Add(monster);
        yield return new WaitForSeconds(DELAY);
        monsters.Remove(monster);
    }
}
