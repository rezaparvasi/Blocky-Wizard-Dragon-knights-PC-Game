using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    private bool stop = false;

	// Use this for initialization
	void Start () {
        ToggleOffGameMenu();
	}
	
	// Update is called once per frame
	void Update () {
        if (stop == false)
        {
            if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
            {
                ToggleGameMenu(); //used to turn on gamemenu icons at start of game
                stop = true;
            }
        }
    }

    //turns off game menu
    public void ToggleOffGameMenu()
    {
        gameObject.SetActive(false);
    }

    //turns on game menu
    public void ToggleGameMenu()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
