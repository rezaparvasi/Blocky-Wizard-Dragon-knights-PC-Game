using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    private Transform target;
    public float RotationSpeed;
    private float timer = 0.0f;
    Quaternion To;

    //values for internal use
    private Quaternion lookRotation;
    private Vector3 direction;

    // Use this for initialization
    void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (target.position.z - transform.position.z < 5 && target.position.z - transform.position.z > -5) //rotates to target only if within range
        {
            //find the vector pointing from our position to the target
            direction = (target.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            lookRotation = Quaternion.LookRotation(direction);

            lookRotation.x = 0; //keeps the x-axis 0
            lookRotation.z = 0; //keeps the z-axis 0

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        }
        else
        {
            if (timer > Random.Range(3, 7)) //timer resets at value
            {
                To = Quaternion.Euler(0.0f, Random.Range(-180.0f, 180.0f), 0.0f);
                timer = 0.0f;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, To, Time.deltaTime * RotationSpeed);
        }
    }
}
