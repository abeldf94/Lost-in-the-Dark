using UnityEngine;

/// <summary>
/// This class manage the sword collider, it will be enabled when attacking animation is doing.
/// If we don't do in that way, the sword will always hit the player when walking between enemies
/// instead just when the monster attack.
/// </summary>
public class ItemAttackHandler : MonoBehaviour {

    // The weapon collider
    public Collider weaponCollider;

    // Enable the collider that sword has
    public void EnableItemToAttack() {
        weaponCollider.enabled = true;
    }

    // Disable the collider that sword has
    public void DisableItemToAttack() {
        weaponCollider.enabled = false;
    }
}
