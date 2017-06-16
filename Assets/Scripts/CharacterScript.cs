using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScript : MonoBehaviour {
    private GameObject thePlayer;
    private GameObject level;
    private GameObject coinParent;
    private int count = 0; //a simple variable to stop the audio sound from repeating
    public DeathMenu deathMenu;
    public GameMenu gameMenu;
    public Generation generationScript;
    private bool isDead = false;
    Bounce bounceScript;

    // Use this for initialization
    void Start () {
        thePlayer = GameObject.Find("ParentPlayer");
        level = GameObject.Find("Level");
        coinParent = GameObject.Find("CoinParent");
        bounceScript = thePlayer.GetComponent<Bounce>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Death()
    {
        thePlayer.GetComponent<Bounce>().isDead = true;
        thePlayer.GetComponent<AnimationController>().isDead = true;
        level.transform.FindChild("Main Camera").GetComponent<CameraFollow>().isDead = true;

        if (PlayerPrefs.GetInt("HighScore") < generationScript.score) //saves the highscore value
            PlayerPrefs.SetInt("HighScore", generationScript.score);

        PlayerPrefs.SetInt("Coins", generationScript.coinsAmount); //saves the coins amount value

        deathMenu.ToggleEndMenu();
        gameMenu.ToggleOffGameMenu();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (isDead == false)
        {
            if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Train") /**DO NOT DELETE THIS IS COMMENTED FOR TESTING **/
            {
                thePlayer.transform.position = thePlayer.GetComponent<Bounce>().endPos; //makes player be at a whole number when dead
                if (count == 0) //makes it so this will only play once when player dies
                {
                    gameObject.GetComponent<AudioSource>().Play();
                    count++;
                }
                Death();
                isDead = true;
            }
            if (col.gameObject.tag == "Coin")
            {
                level.GetComponent<Generation>().coinsAmount++; //increases total coin amount
                coinParent.GetComponent<AudioSource>().Play();
                Destroy(col.gameObject);
            }
            if(col.gameObject.tag == "Object")
            {
                bounceScript.endPos = bounceScript.startPos;
            }
            //if (col.gameObject.tag == "Restart")
            //{
            //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //}
        }
    }
}
