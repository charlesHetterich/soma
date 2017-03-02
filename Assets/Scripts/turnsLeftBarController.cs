using UnityEngine;
using System.Collections;

public class turnsLeftBarController : MonoBehaviour {

    float maxScaleX;
    float screenWidthProportion;
    GameObject board;
    public GameObject menu;

    // Use this for initialization
    void Start () {

        maxScaleX = transform.localScale.x;
        print("screen width: " + Screen.width.ToString());
        float baseProportion = 1024f / 768f;
        float screenProportion = (float)Screen.width / (float)Screen.height;
        screenWidthProportion = screenProportion / baseProportion;


        transform.localScale = new Vector3(0, transform.localScale.y, 1);
    
        board = GameObject.Find("Board");
	}
	
	// Update is called once per frame
	void Update () {
        if (menu.GetComponent<MenuController>().currentMenuType != -1 || board.GetComponent<GameBoardController>().turnsLeft == -1)
            transform.localScale = new Vector3(transform.localScale.x + (0 - transform.localScale.x) / 5, transform.localScale.y, 1);
        else
        {
            float percentTurnsLeft = (float)(board.GetComponent<GameBoardController>().turnsLeft) / (float)(board.GetComponent<GameBoardController>().maxTurns[board.GetComponent<GameBoardController>().level - 1]);

            if (percentTurnsLeft >= 0f && percentTurnsLeft <= 1f)
                transform.localScale = new Vector3(transform.localScale.x + (maxScaleX * screenWidthProportion * percentTurnsLeft - transform.localScale.x) / 5, transform.localScale.y, 1);
        }
    }
}
