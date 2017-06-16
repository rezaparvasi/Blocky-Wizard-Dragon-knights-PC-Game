using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

    //private Transform title;
    //private string characterName = "";

    // Use this for initialization
    void Start () {
        ToggleStartMenu();

        //characterName = GameObject.Find("Level").GetComponent<Generation>().characterName;
        //Transform mapTitles = canvas.transform.FindChild("Map Titles");
        //title = mapTitles.transform.FindChild(name);
        //title.gameObject.SetActive(true); //used to turn on map title
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
        {
            ToggleOffStartMenu(); //used to turn off startmenu icons at start of game
            //title.gameObject.SetActive(false); //used to turn off map title
        }
    }

    //turns on start menu
    public void ToggleStartMenu()
    {
        gameObject.SetActive(true);
    }

    //turns off start menu
    public void ToggleOffStartMenu()
    {
        gameObject.SetActive(false);
    }
}
