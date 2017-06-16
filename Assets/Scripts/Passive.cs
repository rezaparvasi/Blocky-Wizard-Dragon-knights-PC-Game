using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Character")
        {
            Physics.IgnoreLayerCollision(8, 9);
        }
    }
}
