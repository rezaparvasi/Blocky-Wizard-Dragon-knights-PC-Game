using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    public bool isDead = false;
    AudioSource aud;

    void Start()
    {
        aud = GameObject.Find("ParentPlayer").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        //stops the update if player is dead
        if (isDead)
            return;

        //rotates the character based on input direction
        if (Input.GetButtonDown("right"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            aud.Play(); //plays audio clip
        }
        if (Input.GetButtonDown("left"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            aud.Play(); //plays audio clip
        }
        if (Input.GetButtonDown("up"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            aud.Play(); //plays audio clip
        }
        if (Input.GetButtonDown("down"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            aud.Play(); //plays audio clip
        }
    }
}
