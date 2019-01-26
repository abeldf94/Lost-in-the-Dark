using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBody : MonoBehaviour {

    public Transform vrCamera;

    // Update is called once per frame
    void LateUpdate () {
        vrCamera.position = transform.position;;
	}
}
