using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

    public RectTransform panel; //holds the scrollpanel
    public GameObject[] bttn; //holds all of the RawImages
    public RectTransform center; //center to compare the distance for each button
    private float[] distance; //all buttons distance to the center
    private bool dragging = false; //will be true while dragging panel
    private int bttnDistance; //will hold the distance between the buttons
    public int minButtonNum; //holds the number of the button, with smallest distance to center
    public string charName = ""; //holds the current active character name
    public Text nameText;
    AudioSource aud;

	// Use this for initialization
	void Start () {
        //aud = gameObject.GetComponent<AudioSource>();
        int bttnLength = bttn.Length;
        distance = new float[bttnLength];

        //Get distance between buttons
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

        //Generation generationScript = GameObject.Find("Level").GetComponent<Generation>();
        charName = Generation.characterName; //initializes the current active character's name
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < bttn.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance); //Get the min distance

        for(int a = 0; a < bttn.Length; a++)
        {
            if(minDistance == distance[a])
            {
                minButtonNum = a; //holds the number of the button that has the smallest distance to the center
            }
        }

        //based on minButtonNum, this chooses which character is being shown on the character selection screen and changes variable charName
        if (minButtonNum == 0)
        {
            charName = "Frost";
            nameText.text = "Frost";
        }
        if (minButtonNum == 1)
        {
            charName = "Merlin";
            nameText.text = "Merlin";
        }
        if (minButtonNum == 2)
        {
            charName = "MoonMan";
            nameText.text = "MoonMan";
        }
        if (minButtonNum == 3)
        {
            charName = "RedHot";
            nameText.text = "RedHot";
        }
        if (minButtonNum == 4)
        {
            charName = "HammerThis";
            nameText.text = "HammerThis";
        }
        if (minButtonNum == 5)
        {
            charName = "Enchantress";
            nameText.text = "Enchantress";
        }

        if (!dragging)
        {
            LerpToBttn(minButtonNum * -bttnDistance);
        }
	}

    void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void RandIndex()
    {
        int rand = Random.Range(0, bttn.Length);
        minButtonNum = rand;

        if (minButtonNum == 0)
            charName = "Frost";
        if (minButtonNum == 1)
            charName = "Merlin";
        if (minButtonNum == 2)
            charName = "MoonMan";
        if (minButtonNum == 3)
            charName = "RedHot";
        if (minButtonNum == 4)
            charName = "HammerThis";
        if (minButtonNum == 5)
            charName = "Enchantress";
    }
}
