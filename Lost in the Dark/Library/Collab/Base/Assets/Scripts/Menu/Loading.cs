using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour {

    private readonly float TIME = 0.2f;
    private readonly string LOADING = "Loading";
    private readonly string DOT = ".";

    private readonly int MAX_DOT = 3;

	// Use this for initialization
	void Start () {
        StartCoroutine(AnimationSimulation());
	}

    IEnumerator AnimationSimulation() {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        while (gameObject.activeSelf == true) {
            text.text = LOADING;
            for (int i = 0; i < 3; i++) {
                yield return new WaitForSeconds(TIME);
                text.text += DOT;
            }
            yield return new WaitForSeconds(TIME);
        }
    }
}
