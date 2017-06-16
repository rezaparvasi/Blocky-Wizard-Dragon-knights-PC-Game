using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ToggleOffEndMenu();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    //turns on end menu or death menu
    public void ToggleEndMenu()
    {
        gameObject.SetActive(true);
    }

    public void ToggleOffEndMenu()
    {
        gameObject.SetActive(false);
    }

    //restarts currently active scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
