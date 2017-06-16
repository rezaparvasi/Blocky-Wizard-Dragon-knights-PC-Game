using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject thePlayer;
    Vector3 shouldPos = new Vector3(0, 0, 0);
    public bool isDead = false;
    private bool move = false;
    private float speed = 0.5f;
    public DeathMenu deathMenu;
    public GameMenu gameMenu;

    // Update is called once per frame
    void Update () {

        if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
        {
            move = true;
        }

        if (move)
        {
            if (!isDead)
            {
                //*thePlayer's Z position will always be greater then the camera's Z position
                if (gameObject.transform.position.z < thePlayer.transform.position.z - 1)
                {
                    //lerps the camera towards the player
                    shouldPos = Vector3.Lerp(gameObject.transform.position, thePlayer.transform.position, Time.deltaTime);
                    gameObject.transform.position = new Vector3(shouldPos.x, 1, shouldPos.z);
                }
                else
                {
                    //slowly moves the camera forward until player is a certain amount pass the camera's Z position
                    gameObject.transform.position += Vector3.forward * (Time.deltaTime * speed);
                    gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, thePlayer.transform.position.x, Time.deltaTime), 1, gameObject.transform.position.z);
                }
                if(gameObject.transform.position.x <= -1)
                {
                    gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, -1, Time.deltaTime), 1, gameObject.transform.position.z);
                }
                if (gameObject.transform.position.x >= 1)
                {
                    gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, 1, Time.deltaTime), 1, gameObject.transform.position.z);
                }
            }
            if (gameObject.transform.position.z > thePlayer.transform.position.z + 5)
            {
                //sets isDead to true and snaps camera to the player's position
                isDead = true;
                thePlayer.GetComponent<Bounce>().isDead = true;
                thePlayer.GetComponent<AnimationController>().isDead = true;
                shouldPos = Vector3.Lerp(gameObject.transform.position, thePlayer.transform.position, Time.deltaTime);
                gameObject.transform.position = new Vector3(shouldPos.x, 1, shouldPos.z - 4);
                deathMenu.ToggleEndMenu(); //calls the death menu
                gameMenu.ToggleOffGameMenu(); //turns off game menu icons
            }
        }
        //this will snap the camera to the player's position when dead
        if (isDead)
        {
            shouldPos = Vector3.Lerp(gameObject.transform.position, thePlayer.transform.position, Time.deltaTime * 2);
            gameObject.transform.position = new Vector3(shouldPos.x, 1, shouldPos.z);
        }
    }
}
