using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour {


	// Use this for initialization
	void Start () {
        ToggleOffCharacterMenu();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //turns on character menu
    public void ToggleCharacterMenu()
    {
        gameObject.SetActive(true);
    }

    //turns off character menu
    public void ToggleOffCharacterMenu()
    {
        gameObject.SetActive(false);
    }

    //restarts currently active scene
    public void Restart()
    {
        ScrollRectSnap scrollRectSnapScript = GameObject.Find("GameController").GetComponent<ScrollRectSnap>(); //creates a variable
        Generation.characterName = scrollRectSnapScript.charName; //sets the current active character's name in the Generation script

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reloads scene
    }
}
