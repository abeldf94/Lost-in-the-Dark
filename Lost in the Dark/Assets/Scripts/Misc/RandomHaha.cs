using UnityEngine;

public class RandomHaha : MonoBehaviour {

    private void Start() {
        InvokeRepeating("CallHaha", Random.Range(60f, 120f), Random.Range(60f, 120f));
    }

    private void CallHaha() {
        SoundManager.PlayWitch();
    }
}
