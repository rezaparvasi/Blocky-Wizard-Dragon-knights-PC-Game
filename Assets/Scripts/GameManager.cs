using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f; //Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f; //Delay between each Player turn.
    public int playerFoodPoints = 100; //Starting value for Player food points.
    public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
    [HideInInspector] public bool playersTurn = true; //Boolean to check if it's players turn, hidden in inspector but public.

    //private List<Enemy> enemies; //List of all Enemy units, used to issue them move commands.
    private bool enemiesMoving; //Boolean to check if enemies are moving.

    //Awake is always called before any Start functions
    //void Awake()
    //{
    //    //Check if instance already exists
    //    if (instance == null)

    //        //if not, set instance to this
    //        instance = this;

    //    //If instance already exists and it's not this:
    //    else if (instance != this)

    //        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
    //        Destroy(gameObject);

    //    //Sets this to not be destroyed when reloading scene
    //    DontDestroyOnLoad(gameObject);
    //}

    ////Update is called every frame.
    //void Update()
    //{
    //    //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
    //    if (playersTurn || enemiesMoving)

    //        //If any of these are true, return and do not start MoveEnemies.
    //        return;

    //    //Start moving enemies.
    //    StartCoroutine(MoveEnemies());
    //}

    ////Call this to add the passed in Enemy to the List of Enemy objects.
    ////public void AddEnemyToList(Enemy script)
    ////{
    ////    //Add Enemy to List enemies.
    ////    enemies.Add(script);
    ////}

    ////Coroutine to move enemies in sequence.
    //IEnumerator MoveEnemies()
    //{
    //    //While enemiesMoving is true player is unable to move.
    //    enemiesMoving = true;

    //    //Wait for turnDelay seconds, defaults to .1 (100 ms).
    //    yield return new WaitForSeconds(turnDelay);

    //    //If there are no enemies spawned (IE in first level):
    //    if (enemies.Count == 0)
    //    {
    //        //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
    //        yield return new WaitForSeconds(turnDelay);
    //    }

    //    //Loop through List of Enemy objects.
    //    for (int i = 0; i < enemies.Count; i++)
    //    {
    //        //Call the MoveEnemy function of Enemy at index i in the enemies List.
    //        enemies[i].MoveEnemy();

    //        //Wait for Enemy's moveTime before moving next Enemy, 
    //        yield return new WaitForSeconds(enemies[i].moveTime);
    //    }
    //    //Once Enemies are done moving, set playersTurn to true so player can move.
    //    playersTurn = true;

    //    //Enemies are done moving, set enemiesMoving to false.
    //    enemiesMoving = false;
    //}
}
