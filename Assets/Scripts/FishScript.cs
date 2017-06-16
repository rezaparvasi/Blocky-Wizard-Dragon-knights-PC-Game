using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    public float frequency = 1.0f;
    public float amplitude = 1.0f;
    Vector3 startPos;
    private float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime * Time.timeScale * frequency;
        transform.position = startPos + Vector3.up * Mathf.Sin(elapsedTime) * amplitude;
    }
}
