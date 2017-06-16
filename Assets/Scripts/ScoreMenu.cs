using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMenu : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //ToggleOffScoreMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
        {
            ToggleScoreMenu(); //used to turn on gamemenu icons at start of game
        }
    }

    //turns off game menu
    public void ToggleOffScoreMenu()
    {
        gameObject.SetActive(false);
    }

    //turns on game menu
    public void ToggleScoreMenu()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
