using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSpeedSpawn : MonoBehaviour {
    public Vector3 intPos = new Vector3(0, 0, 0); //declares and initializes
    private Transform highSpeedParentTransform; //the transform of the HighSpeedParent gameobject
    public float minDelay = 10f;
    public float maxDelay = 10f;
    private Transform characterTransform; //the transform of your character
    float rand;

    // Use this for initialization
    void Start()
    {
        rand = Random.Range(minDelay, maxDelay);
        intPos = transform.position;
        highSpeedParentTransform = GameObject.FindGameObjectWithTag("HighSpeedParent").transform;
        Invoke("SpawnHighSpeed", rand); //[REMEMBER]!!! Never call SpawnHighSpeed function without invoke or with a 0 time delay, it will lag whole project
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterTransform.position.z - 16 > gameObject.transform.position.z) //if player gets a certain distance it will make sure the highspeed object gets deleted
            Destroy(gameObject);
    }

    private void SpawnHighSpeed()
    {
        GameObject go;
        GameObject level = GameObject.Find("Level");
        Generation generationScript = level.GetComponent<Generation>();
        go = Instantiate(generationScript.highSpeedPrefab) as GameObject;
        go.transform.position = intPos;
        go.transform.SetParent(highSpeedParentTransform); //instantiated objects go in the HighSpeedParent gameobject
    }
}
