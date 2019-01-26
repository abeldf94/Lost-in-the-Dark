using System.Collections;
using UnityEngine;

public class PlayerDamageIndicator : MonoBehaviour {

    // Max player health depending on game difficulty
    private enum MaxPlayerHealth {
        EASY = 200,
        MEDIUM = 150,
        HARD = 100
    }

    // Player health
    private float playerHealth;
    // Max player health
    private float maxPlayerHealth;

    //amount of blood when taking damage (relative to damage taken (relative to HP remaining))
    private float damageBloodAmount;

    //max amount of blood when not taking damage (relative to HP lost)
    private float maxBloodIndication;

    // How much health recover every second
    private readonly float RECOVER_SPEED = 5;

    // Player death status
    public static bool isDead;

    // The player
    public GameObject player;

    // Sounds for hit and death
    public AudioSource hit;
    public AudioSource death;

    void Start() {
        damageBloodAmount = 3;
        maxBloodIndication = 0.5f;
        isDead = false;
        SetHealth();
        maxPlayerHealth = playerHealth;
    }

    void Update() {
        if (!isDead) {
            RecoverHealth();
            FixHealth();

            BleedBehavior.minBloodAmount = maxBloodIndication * (maxPlayerHealth - playerHealth) / maxPlayerHealth;
        }
    }

    /// <summary>
    /// Select health based on difficulty
    /// </summary>
    private void SetHealth() {
        if (Config.LevelDifficulty == Difficulty.EASY)
            playerHealth = (float)MaxPlayerHealth.EASY;
        else if (Config.LevelDifficulty == Difficulty.MEDIUM)
            playerHealth = (float)MaxPlayerHealth.MEDIUM;
        else if (Config.LevelDifficulty == Difficulty.HARD)
            playerHealth = (float)MaxPlayerHealth.HARD;
    }

    /// <summary>
    /// Apply a given damage to player
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount) {
        if (!isDead) {
            Data.DamageReceived += amount;
            hit.Play();

            BleedBehavior.BloodAmount += Mathf.Clamp01(damageBloodAmount * amount / playerHealth);

            playerHealth -= amount;

            if (playerHealth <= 0) {
                StartCoroutine(PlayDeathSound());
                player.GetComponentInChildren<Weapon>().enabled = false;
                isDead = true;
            }

            BleedBehavior.minBloodAmount = maxBloodIndication * (maxPlayerHealth - playerHealth) / maxPlayerHealth;
        }
    }

    /// <summary>
    /// Recover health for player
    /// </summary>
    private void RecoverHealth() {
        if (playerHealth < maxPlayerHealth)
            playerHealth += RECOVER_SPEED * Time.deltaTime;
    }

    /// <summary>
    /// Fix health in case player health is greater than max allowed
    /// It could happen if the recover health is 5 each second and player
    /// has 99 in that moment
    /// </summary>
    private void FixHealth() {
        if (playerHealth > maxPlayerHealth)
            playerHealth = maxPlayerHealth;
    }

    /// <summary>
    /// Play the sound first before make the game transition to the other scene
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDeathSound() {
        death.Play();
        yield return new WaitWhile(() => death.isPlaying);
        GameController.UpdateDeath();
    }

    /// <summary>
    /// Reset the player health
    /// </summary>
    public void ResetHealth() {
        playerHealth = maxPlayerHealth;
    }
}
