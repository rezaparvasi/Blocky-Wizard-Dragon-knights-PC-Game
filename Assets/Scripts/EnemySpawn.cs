using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour{
    public Vector3 intPos = new Vector3(0, 0, 0);
    private Transform enemyParentTransform; //the transform of the EnemyParent gameobject
    public float minDelay = 3f;
    public float maxDelay = 6f;
    private Transform characterTransform; //the transform of your character

    // Use this for initialization
    void Start()
    {
        intPos = transform.position;
        enemyParentTransform = GameObject.FindGameObjectWithTag("EnemyParent").transform;
        Invoke("SpawnEnemy", Random.Range(minDelay, maxDelay)); //[REMEMBER]!!! Never call SpawnEnemy function without invoke or with a 0 time delay, it will lag whole project
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //makes sure any enemy objects that are instantiated are instantiated at the right X position based on their rotation
        if (gameObject.transform.rotation == Quaternion.Euler(0, 90, 0))
            intPos.x = -10; //make sure the number is the same as spawnX on the Generation script (not including the '-' sign)
        else
            intPos.x = 10;  //make sure the number is the same as spawnX on the Generation script

        if (characterTransform.position.z - 16 > gameObject.transform.position.z) //if player gets a certain distance it will make sure the enemy gets deleted
            Destroy(gameObject);
    }

    private void SpawnEnemy()
    {
        GameObject go;
        go = Instantiate(gameObject) as GameObject;
        go.transform.position = intPos;
        go.transform.SetParent(enemyParentTransform); //instantiated objects go in the EnemyParent gameobject
    }
}

