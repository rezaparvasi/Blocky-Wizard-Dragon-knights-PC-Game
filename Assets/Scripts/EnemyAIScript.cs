using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour {

    AudioSource aud;
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

    //private Animator animator; //Variable of type Animator to store a reference to the enemy's Animator component.
    //private Transform target; //Transform to attempt to move toward each turn.
    //private bool skipMove; //Boolean to determine whether or not enemy should skip a turn or move this turn.

    public Vector3 startPos;
    public Vector3 endPos;

    Vector3 startScale;
    Vector3 endScale;

    // Use this for initialization
    void Start () {
        ////Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
        ////This allows the GameManager to issue movement commands.
        //GameManager.instance.AddEnemyToList(this);

        ////Get and store a reference to the attached Animator component.
        //animator = GetComponent<Animator>();

        ////Find the Player GameObject using it's tag and store a reference to its transform component.
        //target = GameObject.FindGameObjectWithTag("Player").transform;

        ////Call the start function of our base class MovingObject.
        //base.Start();

        aud = gameObject.GetComponent<AudioSource>();
        level = GameObject.Find("Level");
        anim = gameObject.GetComponent<Animation>();
        child = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
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

    void FixedUpdate()
    {
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Object")
        {
            endPos = startPos;
        }
    }

    ////Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
    ////See comments in MovingObject for more on how base AttemptMove function works.
    //protected override void AttemptMove<T>(int xDir, int yDir, int zDir)
    //{
    //    //Check if skipMove is true, if so set it to false and skip this turn.
    //    if (skipMove)
    //    {
    //        skipMove = false;
    //        return;

    //    }

    //    //Call the AttemptMove function from MovingObject.
    //    base.AttemptMove<T>(xDir, yDir, zDir);

    //    //Now that Enemy has moved, set skipMove to true to skip next move.
    //    skipMove = true;
    //}


    ////MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    //public void MoveEnemy()
    //{
    //    //Declare variables for X and Y axis move directions, these range from -1 to 1.
    //    //These values allow us to choose between the cardinal directions: up, down, left and right.
    //    int xDir = 0;
    //    int yDir = 1;
    //    int zDir = 0;

    //    //If the difference in positions is approximately zero (Epsilon) do the following:
    //    if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

    //        //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
    //        zDir = target.position.y > transform.position.y ? 1 : -1;

    //    //If the difference in positions is not approximately zero (Epsilon) do the following:
    //    else
    //        //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
    //        xDir = target.position.x > transform.position.x ? 1 : -1;

    //    //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
    //    AttemptMove<Player>(xDir, yDir, zDir);
    //}


    ////OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    ////and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
    //protected override void OnCantMove<T>(T component)
    //{
    //    //Declare hitPlayer and set it to equal the encountered component.
    //    Player hitPlayer = component as Player;

    //    //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
    //    hitPlayer.LoseFood(playerDamage);

    //    //Set the attack trigger of animator to trigger Enemy attack animation.
    //    animator.SetTrigger("enemyAttack");

    //}
}
