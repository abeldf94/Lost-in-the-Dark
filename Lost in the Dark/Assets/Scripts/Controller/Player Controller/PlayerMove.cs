using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

    // Player speed
    private float playerSpeed;

    // Input detected 
    private float vertical;
    private float horizontal;

    private void Start() {
        playerSpeed = 3f;
    }

    // Update is called once per frame
    void Update () {
        if (!PlayerDamageIndicator.isDead) {
            CheckInput();
            ApplyInput();
            CheckFootStep();
        }
	}

    // Check for input
    void CheckInput() {
        horizontal = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
    }

    // Apply the current input
    void ApplyInput() {
        transform.Translate(horizontal, 0, vertical);
    }

    void CheckFootStep() {
        if ((horizontal != 0 || vertical != 0) && (!SoundManager.IsPlayingFootstep()))
            SoundManager.PlayFootstep();
        else if ((horizontal == 0 && vertical == 0) && (SoundManager.IsPlayingFootstep()))
            SoundManager.StopFootstep();                
    }

    public void SetSpeed(float speed) {
        playerSpeed = speed;
    }

    public float GetSpeed() {
        return playerSpeed;
    }
}
