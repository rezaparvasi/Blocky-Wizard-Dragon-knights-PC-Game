using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject Projectile_Emitter;

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Projectile;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float Projectile_Forward_Force;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //The Projectile instantiation happens here.
            GameObject Temporary_Projectile_Handler;
            Temporary_Projectile_Handler = Instantiate(Projectile, Projectile_Emitter.transform.position, Projectile_Emitter.transform.rotation) as GameObject;

            //Sometimes projectiles may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            //Temporary_Projectile_Handler.transform.Rotate(Vector3.left * 90);

            //Retrieve the Rigidbody component from the instantiated Projectile and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Projectile_Handler.GetComponent<Rigidbody>();

            //Tell the projectile to be "pushed" forward by an amount set by Projectile_Forward_Force.
            Temporary_RigidBody.AddForce(transform.forward * Projectile_Forward_Force);

            //Basic Clean Up, set the Projectiles to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Projectile_Handler, 5.0f);
        }
    }
}
