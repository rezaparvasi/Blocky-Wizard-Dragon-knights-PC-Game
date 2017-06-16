using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour {
    private Transform characterTransform; //the transform of your character

    // Use this for initialization
    void Start () {
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (characterTransform.position.z - 16 > gameObject.transform.position.z) //if player gets a certain distance it will make sure the object gets deleted
            Destroy(gameObject);
    }
}
