using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {
    public float lerpSpeed = 5;
    public float scaleSpeed = 3;
    public Transform child;
    public float scaleChange = 0.1f;
    public AnimationCurve ac;
    public Animation anim;

    private float currentLerpTime; //the current time while lerping
    private float currentScaleTime;
    private float perc = 1; //the percentage of lerping that's done
    private float scalePerc;
    private bool FirstInput;
    public bool isDead = false;
    private string characterName = "";
    private GameObject level;  

    public Vector3 startPos;
    public Vector3 endPos;

    Vector3 startScale;
    Vector3 endScale;

    void Start()
    {
        characterName = Generation.characterName;
        level = GameObject.Find("Level");
        anim = gameObject.transform.FindChild(characterName).GetComponent<Animation>(); //make sure name in "" is exact name as character object
        child = gameObject.transform.FindChild(characterName); //make sure name in "" is exact name as character object
    }

    void Update()
    {
        //if the player is below map or fell though water tile, this will stop ALL movement and rotation *DO NOT DELETE THIS IS COMMENTED FOR TESTING*
        //if (child.position.y < 0)
        //{
        //    Death();
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //stops the update if player is dead
        if (isDead)
        {
            child.transform.localScale = new Vector3(1.5f, 0.1f, 1.5f); //makes sure when player is dead its transform is squished
            return;
        }

        if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
        {
            currentScaleTime = 0;
        }
        //when player is holding down key
        if (Input.GetButton("up") || Input.GetButton("down") || Input.GetButton("left") || Input.GetButton("right"))
        {
            startScale = gameObject.transform.localScale;
            endScale = new Vector3(transform.localScale.x + scaleChange, transform.localScale.y - scaleChange, transform.localScale.z);
            FirstInput = true;
        }
        //when player releases key
        if (Input.GetButtonUp("up") || Input.GetButtonUp("down") || Input.GetButtonUp("left") || Input.GetButtonUp("right"))
        {
            //resets the variables
            if (perc >= 1)
            {
                anim.Play("Jump"); //make sure name in "" in exact name as the animation
                currentLerpTime = 0;
                currentScaleTime = 0;
                perc = 0;
                startPos = gameObject.transform.position;
                startScale = gameObject.transform.localScale;
                endScale = new Vector3(1, 1, 1);
            }
        }
        if (Input.GetButtonUp("right") && gameObject.transform.position == endPos)
        {
            endPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
        if (Input.GetButtonUp("left") && gameObject.transform.position == endPos)
        {
            endPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        if (Input.GetButtonUp("up") && gameObject.transform.position == endPos)
        {
            endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
        if (Input.GetButtonUp("down") && gameObject.transform.position == endPos)
        {
            endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }

        //controls lerping
        if (FirstInput == true)
        {
            currentLerpTime += Time.deltaTime * lerpSpeed;
            perc = currentLerpTime;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, ac.Evaluate(perc));

            currentScaleTime += Time.deltaTime * scaleSpeed;
            scalePerc = currentScaleTime;
            child.transform.localScale = Vector3.Lerp(startScale, endScale, ac.Evaluate(scalePerc));
        }
    }

    private void Death()
    {
        isDead = true; //sets isDead to true
        gameObject.GetComponent<AnimationController>().isDead = true; //sets isDead in this gameobject's AnimationController script to true
        level.transform.FindChild("Main Camera").GetComponent<CameraFollow>().isDead = true; //sets isDead in the Main Camera's CameraFollow script to true
    }

    //destroys coin prefabs on collision
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Object")
        {
            endPos = startPos;
        }

        //if (col.gameObject.tag == "Water") /**DO NOT DELETE THIS IS COMMENTED FOR TESTING **/
        //{
        //    col.gameObject.GetComponent<BoxCollider>().enabled = false;
        //}
    }
}
