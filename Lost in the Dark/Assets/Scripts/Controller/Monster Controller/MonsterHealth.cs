using UnityEngine;

public class MonsterHealth : MonoBehaviour {

    // Max monster health based on difficulty
    private enum MonsterMaxHealth {
        EASY = 25,
        MEDIUM = 40,
        HARD = 75
    }

    private enum MonsterTypeMultiplier {
        SPIDER = 1,
        SKELETON = 3,
        TROLL = 10
    }

    // Monster health
    private float monsterHealth;

    // Monther death status
    private bool isDead;

    // Use this for initialization
    void Start() {
        isDead = false;
        SetHealth();
    }

    /// <summary>
    /// Damage received by player
    /// </summary>
    /// <param name="damage"> Amount of damage </param>
    public void Damage(float damage) {
        if (damage < 50) {// This mean the damage is not whit powerups
            Data.DamageGiven += damage;
            SoundManager.PlayHitMarker();
        }
        if (!isDead) {
            monsterHealth -= damage;

            if (monsterHealth <= 0.0f) {
                isDead = true;
                GetComponent<MonsterController>().MonsterDeathItem();
                Data.Kills++;
            }
        }
    }

    /// <summary>
    /// Set the monster health based on game difficulty and the type of monster
    /// </summary>
    private void SetHealth() {
        if (Config.LevelDifficulty == Difficulty.EASY) {
            if (gameObject.tag == "Troll")
                monsterHealth = (float)(MonsterMaxHealth.EASY) * (int)(MonsterTypeMultiplier.TROLL);
            else if (gameObject.tag == "Skeleton")
                monsterHealth = (float)(MonsterMaxHealth.EASY) * (int)(MonsterTypeMultiplier.SKELETON);
            else if (gameObject.tag == "Spider")
                monsterHealth = (float)(MonsterMaxHealth.EASY) * (int)(MonsterTypeMultiplier.SPIDER);
        } else if (Config.LevelDifficulty == Difficulty.MEDIUM) {
            if (gameObject.tag == "Troll")
                monsterHealth = (float)(MonsterMaxHealth.MEDIUM) * (int)(MonsterTypeMultiplier.TROLL);
            else if (gameObject.tag == "Skeleton")
                monsterHealth = (float)(MonsterMaxHealth.MEDIUM) * (int)(MonsterTypeMultiplier.SKELETON);
            else if (gameObject.tag == "Spider")
                monsterHealth = (float)(MonsterMaxHealth.MEDIUM) * (int)(MonsterTypeMultiplier.SPIDER);
        } else if (Config.LevelDifficulty == Difficulty.HARD) {
            if (gameObject.tag == "Troll")
                monsterHealth = (float)(MonsterMaxHealth.HARD) * (int)(MonsterTypeMultiplier.TROLL);
            else if (gameObject.tag == "Skeleton")
                monsterHealth = (float)(MonsterMaxHealth.HARD) * (int)(MonsterTypeMultiplier.SKELETON);
            else if (gameObject.tag == "Spider")
                monsterHealth = (float)(MonsterMaxHealth.HARD) * (int)(MonsterTypeMultiplier.SPIDER);
        }
    }

    /// <summary>
    /// Set the monster status on dead
    /// </summary>
    public bool IsDead() {
        return isDead;
    }
}
