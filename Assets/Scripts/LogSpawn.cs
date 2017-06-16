using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawn : MonoBehaviour {
    public Vector3 intPos = new Vector3(0, 0, 0);
    private Transform logParentTransform; //the transform of the EnemyParent gameobject
    public float minDelay = 3f;
    public float maxDelay = 6f;
    private Transform characterTransform; //the transform of your character

    // Use this for initialization
    void Start()
    {
        intPos = transform.position;
        logParentTransform = GameObject.FindGameObjectWithTag("LogParent").transform;
        Invoke("SpawnLog", Random.Range(minDelay, maxDelay)); //[REMEMBER]!!! Never call SpawnLog function without invoke or with a 0 time delay, it will lag whole project
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ////makes sure any enemy objects that are instantiated are instantiated at the right X position based on their rotation
        //if (gameObject.transform.rotation == Quaternion.Euler(0, 90, 0))
        //    intPos.x = -5; //make sure the number is the same as spawnX on the Generation script (not including the '-' sign)
        //else
        //    intPos.x = 5;  //make sure the number is the same as spawnX on the Generation script

        if (characterTransform.position.z - 16 > gameObject.transform.position.z) //if player gets a certain distance it will make sure the log gets deleted
            Destroy(gameObject);

        //THIS DOESN'T WORK, CHANGE
        //LogMotor logMotor = gameObject.GetComponent<LogMotor>();
        //if (intPos.x <= -5)
        //{
        //    logMotor.forward = Vector3.forward;
        //    transform.rotation = Quaternion.Euler(0, 90, 0);
        //}
    }

    private void SpawnLog()
    {
        GameObject go;
        GameObject level = GameObject.Find("Level");
        Generation generationScript = level.GetComponent<Generation>();
        go = Instantiate(generationScript.logPrefabs[Random.Range(0,generationScript.logPrefabs.Length)]) as GameObject; //the LONG part inside "Instantiate" picks from the logPrefabs array in generation script
        while (go.tag.Equals("Lilypad")) //prevents any lilypads from spawning on a tile where only logs will spawn
        {
            Destroy(go);
            go = Instantiate(generationScript.logPrefabs[Random.Range(0, generationScript.logPrefabs.Length)]) as GameObject;
        }
        //if (transform.position != new Vector3(5, 0.4f, generationScript.spawnZ - 2))
        //{
        //    intPos = new Vector3(5, 0.4f, generationScript.spawnZ - 2);
        //}
        go.transform.position = intPos;
        go.transform.SetParent(logParentTransform); //instantiated objects go in the EnemyParent gameobject
    }

    //private void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag.Equals("Log") || col.tag.Equals("Log1") || col.tag.Equals("Log2"))
    //        Destroy(col.gameObject);
    //}
}
