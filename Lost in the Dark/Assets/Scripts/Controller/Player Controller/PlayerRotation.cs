using UnityEngine;
using UnityEngine.UI;

public class PlayerRotation : MonoBehaviour {

    Transform vrCamera;

    void Start() {
        vrCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.Euler(0, vrCamera.eulerAngles.y, 0);
    }
}
