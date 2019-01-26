using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

	public static void Spawn(Vector3 position, GameObject item) {
        Instantiate(item, position, Quaternion.identity);
    }
}
