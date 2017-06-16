using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadMotor : MonoBehaviour {
    public float speed = 5.0f;
    public int distance = -7; //7
    public Vector3 forward = Vector3.forward;
    private Transform characterTransform; //the transform of your character

    // Use this for initialization
    void Start () {
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        MoveLog();
        if (gameObject.transform.position.x <= distance || gameObject.transform.position.x >= -distance) //if the gameobject goes past a certain point it will destroy it
        {
            Destroy(gameObject);
        }
        if (characterTransform.position.z - 16 > gameObject.transform.position.z) //if player gets a certain distance it will make sure the lilypad gets deleted
            Destroy(gameObject);
    }

    private void MoveLog()
    {
        gameObject.transform.Translate(forward * speed * Time.deltaTime);
    }
}
