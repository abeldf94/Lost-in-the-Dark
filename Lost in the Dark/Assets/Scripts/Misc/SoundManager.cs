using UnityEngine;

/// <summary>
/// Play some sounds when required
/// </summary>
public class SoundManager : MonoBehaviour {

    public AudioSource explosion;
    private static AudioSource explosionSound;

    public AudioSource hitMarker;
    private static AudioSource hitMarkerSound;

    public AudioSource witch;
    private static AudioSource witchSound;

    public AudioSource footstep;
    private static AudioSource footstepSound;

    private void Awake() {
        hitMarkerSound = hitMarker;
        explosionSound = explosion;
        witchSound = witch;
        footstepSound = footstep;
    }

    public static void PlayHitMarker() {
        hitMarkerSound.Play();
    }

    public static void PlayBoom() {
        explosionSound.Play();
    }

    public static void PlayFootstep() {
        footstepSound.Play();
    }

    public static void PlayWitch() {
        witchSound.Play();
    }

    public static void StopFootstep() {
        footstepSound.Stop();
    }

    public static bool IsPlayingFootstep() {
        return footstepSound.isPlaying;
    }
}
