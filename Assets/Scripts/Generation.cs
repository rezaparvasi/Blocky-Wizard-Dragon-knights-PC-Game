using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generation : MonoBehaviour {
    public GameObject[] tilePrefabs; //an array of tile prefabs
    public GameObject[] enemyPrefabs; //an array of enemy prefabs
    public GameObject[] logPrefabs; //an array of log prefabs
    public GameObject[] objectPrefabs; //an array of object prefabs
    public GameObject[] oneTileOuterObjectPrefabs; //an array of 1tile outer object prefabs
    public GameObject highSpeedPrefab; //the highspeed prefab
    public GameObject altSafeZonePrefab; //the safe zone tile that will alternate between it and the spawned safe zone tile
    public GameObject altRoadPrefab; //the road tile that will alternate between it and the spawned road tile
    public GameObject coinPrefab; //the coin prefab
    public GameObject fishPrefab; //the fish prefab
    private GameObject thePlayer; //reference to the ParentPlayer gameobject
    private GameObject level; //reference to the Level gameobject
    public DeathMenu deathMenu;
    private Transform characterTransform; //the transform of your character
    private Transform enemyParentTransform; //the transform of the EnemyParent gameobject
    private Transform logParentTransform; //the transform of the LogParent gameobject
    private Transform highSpeedParentTransform; //the transform of the HighSpeedParent gameobject
    private Transform coinParentTransform; //the transform of the CoinParent gameobject
    private Transform objectParentTransform; //the transform of the ObjectParent gameobject
    private int firstRand; //picks a prefab
    private int secondRand; //picks the number of prefabs to instantiate
    private int disPlayer = -8; //the distance from the player the prefabs will spawn [REMEMBER]!!! (if this is changed, spawnZ is also changed) 9 8 original value
    private int lastPrefabIndex = 0; //index for the last prefab
    private int count = 0; //used to stop spawning tiles if player goes backwards then forwards again, improves performance
    private float safeZone = 8.0f; //the distance it takes the player to move before tiles start deleting 20
    public float spawnZ = -6.0f; //the Z position of the first spawned prefab [REMEMBER]!!! (if this is changed, disPlayer is also changed) 11 -6 original value
    private int amountTilesOnScreen = 50; //the amount of tiles that will constantly be on screen (not completely accurate) //40 original value
    private float tileLength = 1.0f; //the length in the z-axis of the tile or prefab measured by the cubes of the gridlines
    private int num1 = 1; //used in SpawnEnemy()
    private int num2 = 1; //used in SpawnLog()
    private int num3 = 1; //used in SpawnHighSpeed()
    private int num4 = 1; //used in SpawnTile()
    private int num5 = 1; //used in SpawnTile()
    private int spawnX = 10; //used for the X position of instantiated objects such as (logs, enemies)
    public int score = 0; //the player's score
    public int coinsAmount = 0; //the amount of coins the player has
    public Text scoreText; //the score text
    public Text highScoreText; //the highscore text
    public Text coinsAmountText; //the coins amount text
    public static string mapName = "Map1"; //sets default name
    public static string characterName = "Frost"; //sets default character name
    private int counter = 0; //used with changing the map with progression lower in script
    private bool isPaused = false; //a bool to check if game is paused
    private int minTileRange = 1; //the minimum number of tiles to spawn 1
    private int maxTileRange = 6; //the maximum number of tiles to spawn 6
    private int tempo = 0; //used to spawn the beginning safe zone tiles at start, used lower in script
    private int c = 0; //used to spawn tiles when game is started lower in script
    private float offset = 20f; //used in most of the functions that spawn stuff
    private int offset2 = 0; //used in the spawning of the outer objects for tiles, used lower in script

    Vector3 intPos = new Vector3(0, 0, 0);

    private List<GameObject> activeTiles; //list to keep record of all active tiles in scene
    private List<GameObject> activeEnemies; //list to keep record of all active enemies in scene (May not be needed)
    private List<GameObject> activeLogs; //list to keep record of all active logs in scene

    void Start()
    {
        activeTiles = new List<GameObject>(); //creates the list
        activeEnemies = new List<GameObject>(); //creates the list
        activeLogs = new List<GameObject>(); //creates the list
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform; //initializes
        enemyParentTransform = GameObject.FindGameObjectWithTag("EnemyParent").transform; //initializes
        logParentTransform = GameObject.FindGameObjectWithTag("LogParent").transform; //initializes
        highSpeedParentTransform = GameObject.FindGameObjectWithTag("HighSpeedParent").transform; //initializes
        coinParentTransform = GameObject.FindGameObjectWithTag("CoinParent").transform; //initializes
        objectParentTransform = GameObject.FindGameObjectWithTag("ObjectParent").transform; //initializes
        highScoreText.text = "Best : " + PlayerPrefs.GetInt("HighScore").ToString(); //gets the highscore in registry
        coinsAmount = PlayerPrefs.GetInt("Coins"); //gets the coins amount in registry
        thePlayer = GameObject.Find("ParentPlayer");
        thePlayer.transform.FindChild(characterName).gameObject.SetActive(true); //turns on the active named player
        level = GameObject.Find("Level");

        //if the static lists are filled, this puts those prefabs into the arrays
        if (ArrayHolder.tilePrefabs.Count > 0)
        {
            for (int j = 0; j < tilePrefabs.Length; j++)
            {
                tilePrefabs[j] = null;
            }
            for (int k = 0; k < tilePrefabs.Length; k++)
            {
                tilePrefabs[k] = ArrayHolder.tilePrefabs[k];
            }
        }
        if (ArrayHolder.enemyPrefabs.Count > 0)
        {
            for (int j = 0; j < enemyPrefabs.Length; j++)
            {
                enemyPrefabs[j] = null;
            }
            for (int k = 0; k < enemyPrefabs.Length; k++)
            {
                enemyPrefabs[k] = ArrayHolder.enemyPrefabs[k];
            }
        }
    }

    void Update() {
        GameObject parentPlayer = GameObject.Find("ParentPlayer");
        Bounce bounceScript = parentPlayer.GetComponent<Bounce>();
        AnimationController animC = parentPlayer.GetComponent<AnimationController>();
        scoreText.text = score.ToString(); //used to show the score value
        coinsAmountText.text = coinsAmount.ToString(); //used to show the coins amount

        //if player goes past a certain point on the X-axis this will stop the player's movement and rotation (so a death scene can happen)
        if (parentPlayer.transform.position.x <= -5 || parentPlayer.transform.position.x >= 5)
        {
            bounceScript.isDead = true; //stops player movement
            animC.isDead = true; //stops player rotation
            level.transform.FindChild("Main Camera").GetComponent<CameraFollow>().isDead = true;
            parentPlayer.transform.position = bounceScript.endPos;
            deathMenu.ToggleEndMenu();
        }

        //used to generate everything
        if (Input.GetButtonDown("up") && bounceScript.isDead == false || c < 6) //makes sure this part of script doesn't work if player is dead, and it spawns tiles at start
        {
            c++;
            //keeps track of player score and increases counter variable
            if (count == 0)
            {
                if (bounceScript.startPos != bounceScript.endPos) //makes sure score is only going up for forward hops and not hopping with objects blocking the way
                    score++;
                counter++;
            }

            //////////////////////////////////////////THIS CHANGES THE MAP WITH PROGRESSION//////////////////////////////////////////////////////////////
            GameObject so;
            so = GameObject.FindGameObjectWithTag(mapName); //finds the gameobject that has the holder script
            if (counter == 1)
            {
                if (so.GetComponent<HolderScript>().tilePrefabs.Length != 0)
                {
                    for (int j = 0; j < tilePrefabs.Length; j++)
                    {
                        tilePrefabs[j] = null;
                    }
                    for (int k = 0; k < tilePrefabs.Length; k++)
                    {
                        tilePrefabs[k] = so.GetComponent<HolderScript>().tilePrefabs[k];
                    }
                }
                if (so.GetComponent<HolderScript>().enemyPrefabs.Length != 0)
                {
                    for (int k = 0; k < enemyPrefabs.Length; k++)
                    {
                        enemyPrefabs[k] = null;
                    }
                    for (int l = 0; l < enemyPrefabs.Length; l++)
                    {
                        enemyPrefabs[l] = so.GetComponent<HolderScript>().enemyPrefabs[l];
                    }
                }
                if (so.GetComponent<HolderScript>().logPrefabs.Length != 0)
                {
                    for (int j = 0; j < logPrefabs.Length; j++)
                    {
                        logPrefabs[j] = null;
                    }
                    for (int k = 0; k < logPrefabs.Length; k++)
                    {
                        logPrefabs[k] = so.GetComponent<HolderScript>().logPrefabs[k];
                    }
                }
                if (so.GetComponent<HolderScript>().highSpeedPrefab != null)
                {
                    highSpeedPrefab = null;
                    highSpeedPrefab = so.GetComponent<HolderScript>().highSpeedPrefab;
                }
                if (so.GetComponent<HolderScript>().objectPrefabs.Length != 0)
                {
                    for (int k = 0; k < objectPrefabs.Length; k++)
                    {
                        objectPrefabs[k] = null;
                    }
                    for (int l = 0; l < objectPrefabs.Length; l++)
                    {
                        objectPrefabs[l] = so.GetComponent<HolderScript>().objectPrefabs[l];
                    }
                }
                if (so.GetComponent<HolderScript>().altSafeZonePrefab != null)
                {
                    altSafeZonePrefab = null;
                    altSafeZonePrefab = so.GetComponent<HolderScript>().altSafeZonePrefab;
                }
                if (so.GetComponent<HolderScript>().altRoadPrefab != null)
                {
                    altRoadPrefab = null;
                    altRoadPrefab = so.GetComponent<HolderScript>().altRoadPrefab;
                }
                if (so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs.Length != 0)
                {
                    for (int k = 0; k < oneTileOuterObjectPrefabs.Length; k++)
                    {
                        oneTileOuterObjectPrefabs[k] = null;
                    }
                    for (int l = 0; l < oneTileOuterObjectPrefabs.Length; l++)
                    {
                        oneTileOuterObjectPrefabs[l] = so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs[l];
                    }
                }
            }

            if (counter == 25) //50
            {
                //increases the amount of tiles that can be spawned and increases distance before tiles are deleted
                //minTileRange++;
                //maxTileRange++;
                safeZone++;

                if (so.GetComponent<HolderScript>().tilePrefabs2.Length == 0)
                {
                    counter = 0;
                    return;
                }

                if (so.GetComponent<HolderScript>().tilePrefabs2.Length != 0)
                {
                    for (int j = 0; j < tilePrefabs.Length; j++)
                    {
                        tilePrefabs[j] = null;
                    }
                    for (int k = 0; k < tilePrefabs.Length; k++)
                    {
                        tilePrefabs[k] = so.GetComponent<HolderScript>().tilePrefabs2[k];
                    }
                }
                if (so.GetComponent<HolderScript>().enemyPrefabs2.Length != 0)
                {
                    for (int k = 0; k < enemyPrefabs.Length; k++)
                    {
                        enemyPrefabs[k] = null;
                    }
                    for (int l = 0; l < enemyPrefabs.Length; l++)
                    {
                        enemyPrefabs[l] = so.GetComponent<HolderScript>().enemyPrefabs2[l];
                    }
                }
                if (so.GetComponent<HolderScript>().highSpeedPrefab2 != null)
                {
                    highSpeedPrefab = null;
                    highSpeedPrefab = so.GetComponent<HolderScript>().highSpeedPrefab2;
                }
                if (so.GetComponent<HolderScript>().objectPrefabs2.Length != 0)
                {
                    for (int k = 0; k < objectPrefabs.Length; k++)
                    {
                        objectPrefabs[k] = null;
                    }
                    for (int l = 0; l < objectPrefabs.Length; l++)
                    {
                        objectPrefabs[l] = so.GetComponent<HolderScript>().objectPrefabs2[l];
                    }
                }
                if (so.GetComponent<HolderScript>().altSafeZonePrefab2 != null)
                {
                    altSafeZonePrefab = null;
                    altSafeZonePrefab = so.GetComponent<HolderScript>().altSafeZonePrefab2;
                }
                if (so.GetComponent<HolderScript>().altRoadPrefab2 != null)
                {
                    altRoadPrefab = null;
                    altRoadPrefab = so.GetComponent<HolderScript>().altRoadPrefab2;
                }
                if (so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs2.Length != 0)
                {
                    for (int k = 0; k < oneTileOuterObjectPrefabs.Length; k++)
                    {
                        oneTileOuterObjectPrefabs[k] = null;
                    }
                    for (int l = 0; l < oneTileOuterObjectPrefabs.Length; l++)
                    {
                        oneTileOuterObjectPrefabs[l] = so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs2[l];
                    }
                }
            }

            if (counter == 75) //100
            {
                //increases the amount of tiles that can be spawned and increases distance before tiles are deleted
                //minTileRange++;
                //maxTileRange++;
                safeZone++;

                if (so.GetComponent<HolderScript>().tilePrefabs3.Length == 0)
                {
                    counter = 0;
                    return;
                }

                if (so.GetComponent<HolderScript>().tilePrefabs3.Length != 0)
                {
                    for (int j = 0; j < tilePrefabs.Length; j++)
                    {
                        tilePrefabs[j] = null;
                    }
                    for (int k = 0; k < tilePrefabs.Length; k++)
                    {
                        tilePrefabs[k] = so.GetComponent<HolderScript>().tilePrefabs3[k];
                    }
                }
                if (so.GetComponent<HolderScript>().enemyPrefabs3.Length != 0)
                {
                    for (int k = 0; k < enemyPrefabs.Length; k++)
                    {
                        enemyPrefabs[k] = null;
                    }
                    for (int l = 0; l < enemyPrefabs.Length; l++)
                    {
                        enemyPrefabs[l] = so.GetComponent<HolderScript>().enemyPrefabs3[l];
                    }
                }
                if (so.GetComponent<HolderScript>().highSpeedPrefab3 != null)
                {
                    highSpeedPrefab = null;
                    highSpeedPrefab = so.GetComponent<HolderScript>().highSpeedPrefab3;
                }
                if (so.GetComponent<HolderScript>().objectPrefabs3.Length != 0)
                {
                    for (int k = 0; k < objectPrefabs.Length; k++)
                    {
                        objectPrefabs[k] = null;
                    }
                    for (int l = 0; l < objectPrefabs.Length; l++)
                    {
                        objectPrefabs[l] = so.GetComponent<HolderScript>().objectPrefabs3[l];
                    }
                }
                if (so.GetComponent<HolderScript>().altSafeZonePrefab3 != null)
                {
                    altSafeZonePrefab = null;
                    altSafeZonePrefab = so.GetComponent<HolderScript>().altSafeZonePrefab3;
                }
                if (so.GetComponent<HolderScript>().altRoadPrefab3 != null)
                {
                    altRoadPrefab = null;
                    altRoadPrefab = so.GetComponent<HolderScript>().altRoadPrefab3;
                }
                if (so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs3.Length != 0)
                {
                    for (int k = 0; k < oneTileOuterObjectPrefabs.Length; k++)
                    {
                        oneTileOuterObjectPrefabs[k] = null;
                    }
                    for (int l = 0; l < oneTileOuterObjectPrefabs.Length; l++)
                    {
                        oneTileOuterObjectPrefabs[l] = so.GetComponent<HolderScript>().oneTileOuterObjectPrefabs3[l];
                    }
                }
            }

            if (counter == 125) //150
            {
                //increases the amount of tiles that can be spawned and increases distance before tiles are deleted
                //minTileRange++;
                //maxTileRange++;
                safeZone++;

                counter = 0;
            }
            ////////////////////////////////////////////////////////////////////////END/////////////////////////////////////////////////////////////////////

            if (count == 0 && characterTransform.position.z - safeZone > (spawnZ - amountTilesOnScreen * tileLength))
            {
                while (lastPrefabIndex == firstRand) //used so no same prefabs are getting instantiated after each other except the determined number
                    firstRand = Random.Range(0, tilePrefabs.Length);

                secondRand = Random.Range(minTileRange, maxTileRange); //picks the amount of tiles between minTileRange and maxTileRange

                //if (firstRand == 2) //if the tile is highspeed this will spawn exactly the specified number of tiles
                //    secondRand = 5;

                //spawns the beginning safe zone tiles when game starts
                if (tempo == 0)
                {
                    firstRand = 0; //index 0 for safe zone tile
                    secondRand = 11; //number of tiles
                }

                for (int i = 0; i < secondRand; i++)
                {
                    intPos = new Vector3(0, -0.5f, disPlayer - 1f);
                    disPlayer += 1;
                    SpawnTile(); //calls SpawnTile function
                    SpawnCoin(); //calls SpawnCoin function
                    if (activeTiles[activeTiles.Count - 1].tag.Equals("Road")) //makes sure it only spawns enemies on road tiles
                        SpawnEnemy(); //calls SpawnEnemy function
                    if (activeTiles[activeTiles.Count - 1].tag.Equals("Water")) //makes sure it only spawns logs on water tiles
                        SpawnLog(); //calls SpawnLog function
                    if (activeTiles[activeTiles.Count - 1].tag.Equals("HighSpeed")) //makes sure it only spawns highspeed objects on train tiles
                        SpawnHighSpeed(); //calls SpawnLog function
                    spawnZ += tileLength;
                    if (characterTransform.position.z > safeZone) //at a certain point this will start deleting gameobjects
                    {
                        //if (activeTiles[0].tag.Equals("Road")) //if the active tile of element 0 is a road tile then it will delete said enemy on it
                        //    DeleteEnemy(); //calls DeleteEnemy function
                        if (activeTiles[0].tag.Equals("Water")) //if the active tile of element 0 is a water tile then it will delete said log on it
                            DeleteLog(); //calls DeleteLog function
                        DeleteTile(); //calls DeleteTile function
                    }
                }
                //if (activeTiles[activeTiles.Count - 1].tag.Equals("Grass"))
                //    SpawnOuterObjects();
                tempo = 1; //sets tempo3 to 1 so its not used anymore (only used for spawning the first tiles)
                num4 = 1; //resets the num4 variable used in SpawnTile function
                num5 = 1; //resets the num5 variable used in SpawnTile function
            }
            count += 1;
            if (count > 0)
                count = 0;
        }
        if (Input.GetButtonDown("down"))
        {
            count -= 1;
        }
    }

    //randomizes the prefabs that are instantiated so no same prefabs are instantiated after each other (Is Not Currently Used)
    private int RandomPrefabIndex()
    {
        //if list only has 0 or 1 prefab
        if (tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;

        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

    //spawns the tiles
    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        if (prefabIndex == -1 && num4 == 1 && num5 == 1) //prevents any errors from happening and has to do with interchanging tiles with their alternates
            go = Instantiate(tilePrefabs[firstRand]) as GameObject;
        else if (num4 == -1)
            go = Instantiate(altSafeZonePrefab) as GameObject;
        else if (num5 == -1)
            go = Instantiate(altRoadPrefab) as GameObject;
        else
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;

        if (go.tag.Equals("Road")) //makes all road tiles lower (so all tiles starts out at same Y)
            intPos = new Vector3(0, -0.5f, disPlayer - 2f);
        if (go.tag.Equals("HighSpeed"))
            intPos = new Vector3(0, -0.5f, disPlayer - 2f);
        if (go.tag.Equals("Water")) //makes all water tiles lower (so all tiles starts out at same Y)
            intPos = new Vector3(0, -0.5f, disPlayer - 2f);
        if (go.tag.Equals("Grass"))
        {
            num4 = -num4; //used for alternating "grass" tiles
        }
        if (go.tag.Equals("Road"))
        {
            num5 = -num5;
        }

        go.transform.position = intPos;
        go.transform.SetParent(transform); //instantiated objects go in the Level gameobject
        lastPrefabIndex = firstRand;
        activeTiles.Add(go); //adds the instantiated prefab to the active tiles list
        if (go.tag.Equals("Grass"))
        {
            SpawnMiddleObjects(); //a function to spawn objects on safe zone tiles, located lower in script
        }
    }

    //deletes previously instantiated tiles at the beginning of the active tiles list
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0); //removes the prefab from list
    }

    //spawns the enemies
    private void SpawnEnemy(int prefabIndex = -1)
    {
        Vector3 initialPos = new Vector3(0, 0, 0); //initializes
        int thirdRand = Random.Range(0, enemyPrefabs.Length);
        GameObject go;
        if (prefabIndex == -1) //prevents any errors from happening
            go = Instantiate(enemyPrefabs[thirdRand]) as GameObject;
        else
            go = Instantiate(enemyPrefabs[prefabIndex]) as GameObject;
        //this keeps changing the position of the spawned enemies from one side of the tile to the other so they arn't going in only 1 direction
        if (num1 == -1)
        {
            initialPos = new Vector3(spawnX, 0.14f, spawnZ - offset);
        }
        if (num1 == 1)
        {
            initialPos = new Vector3(-spawnX, 0.14f, spawnZ - offset);
            EnemyMotor enemyMotor = go.GetComponent<EnemyMotor>();
            enemyMotor.forward = Vector3.forward; //changes the forward variable in EnemyMotor script to the opposite direction
            go.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        go.transform.position = initialPos;
        go.transform.SetParent(enemyParentTransform); //instantiated objects go in the EnemyParent gameobject
        activeEnemies.Add(go); //adds the instantiated prefab to the active enemies list
        num1 = -num1;
    }

    //deletes previously instantiated enemies at the beginning of the active enemies list
    private void DeleteEnemy()
    {
        Destroy(activeEnemies[0]);
        activeEnemies.RemoveAt(0); //removes the prefab from list
    }

    //spawns the logs
    private void SpawnLog(int prefabIndex = -1)
    {
        Vector3 initialPos = new Vector3(0, 0, 0); //initializes
        int thirdRand = Random.Range(0, logPrefabs.Length); //used to pick a random prefab from array

        GameObject go;
        if (prefabIndex == -1) //prevents any errors from happening
            go = Instantiate(logPrefabs[thirdRand]) as GameObject;
        else
            go = Instantiate(logPrefabs[prefabIndex]) as GameObject;

        //makes it so the logs are random in which direction they will spawn facing
        if (num2 == -1)
        {
            initialPos = new Vector3(spawnX, 0.0f, spawnZ - offset);
        }
        if (num2 == 1)
        {
            if (go.tag.Equals("Lilypad"))
                initialPos = new Vector3(-spawnX, 0.1f, spawnZ - offset - 0.25f);
            else
            {
                initialPos = new Vector3(-spawnX, 0.0f, spawnZ - offset);
                LogMotor logMotor = go.transform.GetComponent<LogMotor>();
                logMotor.forward = Vector3.forward; //changes the forward variable in LogMotor script to the opposite direction
                go.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }

        //makes sure no lilypad objects are being instantiated right after another lilypad object, prevents problems of not being able to jump lilypads (Doesn't work yet)
        //while (go.tag.Equals("Lilypad") && activeLogs[activeLogs.Count - 1].tag.Equals("Lilypad"))
        //{
        //    Destroy(go);
        //    go = Instantiate(logPrefabs[Random.Range(0, logPrefabs.Length)]) as GameObject;
        //    initialPos = new Vector3(5, 0.4f, spawnZ - 2); //default position
        //}

        //if the instantiated object is a lilypad, this will set its position and spawn a fish on its water tile
        if (go.tag.Equals("Lilypad"))
        {
            initialPos = new Vector3(-spawnX / 2 + Random.Range(4, 6), 0.1f, spawnZ - offset - 0.25f);
            //spawns fish
            GameObject mo;
            intPos = new Vector3(Random.Range(-4, 4), 0.4f, spawnZ - offset);
            mo = Instantiate(fishPrefab) as GameObject;
            mo.transform.position = intPos;
            mo.transform.SetParent(objectParentTransform);
        }

        go.transform.position = initialPos;
        go.transform.SetParent(logParentTransform); //instantiated objects go in the LogParent gameobject
        activeLogs.Add(go); //adds the instantiated prefab to the active logs list
        num2 = -num2;
    }

    //deletes previously instantiated logs at the beginning of the active logs list
    private void DeleteLog()
    {
        Destroy(activeLogs[0]);
        activeLogs.RemoveAt(0); //removes the prefab from list
    }

    //spawns the highspeed objects
    private void SpawnHighSpeed()
    {
        Vector3 initialPos = new Vector3(0, 0, 0); //initializes
        GameObject go;
        go = Instantiate(highSpeedPrefab) as GameObject;
        if (num3 == -1)
            initialPos = new Vector3(10, 1.0f, spawnZ - offset);
        if (num3 == 1)
        {
            initialPos = new Vector3(-10, 1.0f, spawnZ - offset);
            HighSpeedMotor highSpeedMotor = go.GetComponent<HighSpeedMotor>();
            highSpeedMotor.forward = Vector3.forward; //changes the forward variable in highSpeedMotor script to the opposite direction
            go.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        go.transform.position = initialPos;
        go.transform.SetParent(highSpeedParentTransform); //instantiated objects go in the HighSpeedParent gameobject
        num3 = -num3;
    }

    //spawns coins randomly throughout the game
    private void SpawnCoin()
    {
        if (activeTiles[activeTiles.Count - 1].tag.Equals("Water")) //doesn't spawn coins on water tiles
            return;
        //makes it so coins don't spawn on the beginning tiles at start
        if (tempo == 0)
            return;
        Vector3 initialPos = new Vector3(0, 0, 0); //initializes
        int thirdRand = Random.Range(1, 10); //makes it a 1/10 chance to spawn a coin when a tile is spawned
        if (thirdRand == 1)
        {
            GameObject go;
            go = Instantiate(coinPrefab) as GameObject;
            initialPos = new Vector3(Random.Range(-4, 4), 1.0f, spawnZ - offset);
            go.transform.position = initialPos;
            go.transform.SetParent(coinParentTransform); //instantiated objects go in the CoinParent gameobject
        }
    }

    private void SpawnMiddleObjects()
    {
        int positionX = -4;
        int rand = Random.Range(0, 2); //the chance that object(s) will be instantiated on a tile
        if (rand == 1)
        {
            for (int i = 0; i < 8; i++) //goes through all the 1-unit spots of the middle of the tile
            {
                int secondRand = Random.Range(0, 5); //the chance that an object will be instantiated on a 1-unit spot
                if (secondRand == 0)
                {
                    GameObject mo;
                    int thirdRand = Random.Range(0, objectPrefabs.Length); //picks a random object from list
                    intPos = new Vector3(positionX + i, 1.0f, spawnZ - offset); //2 original value
                    mo = Instantiate(objectPrefabs[thirdRand]) as GameObject;

                    //spawns the object in a random rotation
                    int fourthRand = Random.Range(0, 3);
                    if (fourthRand == 0)
                        mo.transform.rotation = Quaternion.Euler(0, 90, 0);
                    if (fourthRand == 1)
                        mo.transform.rotation = Quaternion.Euler(0, -90, 0);
                    if (fourthRand == 2)
                        mo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (fourthRand == 3)
                        mo.transform.rotation = Quaternion.Euler(0, 180, 0);

                    mo.transform.position = intPos;
                    mo.transform.SetParent(objectParentTransform); //stores object in ObjectParent
                }
            }
        }
    }

    private void SpawnOuterObjects()
    {
        int numOfTiles = secondRand; //keep track of how many tiles are left to add objects too, this number will repeatedly be decreased
        if(secondRand == 1)
        {
            GameObject go;
            //left side
            intPos = new Vector3(-5f, 1.0f, spawnZ - offset - 1f);
            go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
            go.transform.position = intPos;
            go.transform.SetParent(objectParentTransform);
            //right side
            intPos = new Vector3(5f, 1.0f, spawnZ - offset - 1f); //different because object is being rotated
            go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
            go.transform.rotation = Quaternion.Euler(0, 90, 0);
            go.transform.position = intPos;
            go.transform.SetParent(objectParentTransform);
        }
        else
        {
            float increaseZ = 0;
            float offset3 = 0; //used to keep objects in the right position when spawning
            if (secondRand > 4)
            {
                offset3 = secondRand - 4;
            }
            while (numOfTiles > 0)
            {
                int rand = Random.Range(1, 1); //used to show distance and used for picking which objects
                if (numOfTiles == 1)
                {
                    GameObject go;
                    //left side
                    if (tempo == 0)
                        intPos = new Vector3(-5f, 1.0f, spawnZ - offset - 11f + increaseZ); //-1
                    else
                    {
                        intPos = new Vector3(-5f, 1.0f, spawnZ - offset - 4f + increaseZ - offset3); //-1
                    }
                    go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
                    go.transform.position = intPos;
                    go.transform.SetParent(objectParentTransform);
                    //right side
                    if (tempo == 0)
                        intPos = new Vector3(5f, 1.0f, spawnZ - offset - 11f + increaseZ); //-1
                    else
                    {
                        intPos = new Vector3(5f, 1.0f, spawnZ - offset - 3f + increaseZ - offset3); //-1
                    }
                    go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
                    go.transform.rotation = Quaternion.Euler(0, 90, 0);
                    go.transform.position = intPos;
                    go.transform.SetParent(objectParentTransform);
                    increaseZ = increaseZ + rand;
                    return;
                }               
                if (rand == 1)
                {
                    GameObject go;
                    //left side
                    if (tempo == 0)
                        intPos = new Vector3(-5f, 1.0f, spawnZ - offset - 11f + increaseZ); //-1
                    else
                    {
                        intPos = new Vector3(-5f, 1.0f, spawnZ - offset - 4f + increaseZ - offset3); //-1
                    }
                    go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
                    go.transform.position = intPos;
                    go.transform.SetParent(objectParentTransform);
                    //right side
                    if (tempo == 0)
                        intPos = new Vector3(5f, 1.0f, spawnZ - offset - 11f + increaseZ); //-1
                    else
                    {
                        intPos = new Vector3(5f, 1.0f, spawnZ - offset - 4f + increaseZ - offset3); //-1
                    }
                    go = Instantiate(oneTileOuterObjectPrefabs[Random.Range(0, oneTileOuterObjectPrefabs.Length)]) as GameObject;
                    go.transform.rotation = Quaternion.Euler(0, 90, 0);
                    go.transform.position = intPos;
                    go.transform.SetParent(objectParentTransform);
                }
                numOfTiles -= rand;
                increaseZ += rand;
            }
        }
    }

    void SpawnStartObjects()
    {

    }

    //function to pause the game (used for UI pause button)
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
            return;
        }
        if (!isPaused)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
        }
    }
}
