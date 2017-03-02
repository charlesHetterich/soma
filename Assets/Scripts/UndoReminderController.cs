using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UndoReminderController : MonoBehaviour {

    public GameObject board;
    public GameObject menu;
    float destAlpha = 0;
    float destScale = 1;
    float destY;
    float showPointY;
    float hidePointY;
    string undoRemind = "Press Z";
    string[] encouragement;

    bool noTurnsLastTime = false;

	// Use this for initialization
	void Start () {

        encouragement = new string[]{"Good Job!", "You Rock!", "Well Played.", "Better Than Granny!", "Wonderful!",
                                     "Exquisite.", "Extraordinary!", "Perfect Execution.", "You've got some nice moves!",
                                     "Supreme.", "Divine!", "That's The Way Sonny!", "Bravo!", "GG", "Gnarly!",
                                     "You're On Fire!", "Got 'em", "Radical!", "Fabulous!", "Cool, Cool.", "Place Encouraging Words Here.",
                                     "Most Righteous.", "Nifty!", "Splendid!", "Zesty!", "Yee haw!", ""};

        //position
        destY = transform.position.y;
        showPointY = destY;
        hidePointY = destY + 100;
        transform.position = new Vector3(transform.position.x, hidePointY, transform.position.z);
        destY = hidePointY;

        //color
        //GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0f);
        destAlpha = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        //if no turns left, show text
        if (board.GetComponent<GameBoardController>().turnsLeft == 0 && !board.GetComponent<GameBoardController>().isGoingToWin && menu.GetComponent<MenuController>().currentMenuType == -1)
        {
            destY = showPointY;
            destAlpha = 1f;
            GetComponent<Text>().text = undoRemind;
        }
        else if (board.GetComponent<GameBoardController>().isGoingToWin)
        {
            destY = showPointY;
            destAlpha = 1f;
            if (!noTurnsLastTime)
                GetComponent<Text>().text = encouragement[Random.Range(0, encouragement.Length)];
        }
        else //otherwise, hide text
        {
            destY = hidePointY;
            destAlpha = 0f;
        }

        //approach dest alpha and dest scale
        //GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a + (destAlpha - GetComponent<SpriteRenderer>().color.a) / 10);
        transform.position = new Vector3(transform.position.x, transform.position.y + (destY - transform.position.y) / 5, transform.position.z);

        //set if there were moves last time
        if (board.GetComponent<GameBoardController>().turnsLeft <= 0 || board.GetComponent<GameBoardController>().isGoingToWin)
            noTurnsLastTime = true;
        else
            noTurnsLastTime = false;
    }
}
