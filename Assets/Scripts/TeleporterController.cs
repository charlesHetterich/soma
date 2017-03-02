using UnityEngine;
using System.Collections;

public class TeleporterController : MonoBehaviour {

    public GameObject particleEffect;

    GameObject menu;
    GameObject board;
    public GameObject pairTeleporter;

    float lastTpPulsePair;
    
    float destScale = 0.005f;

    public int pairID = 0;
    public int numPairs = 1;

	// Use this for initialization
	void Start () {
        menu = GameObject.Find("Menu");
        board = GameObject.Find("Board");

        transform.localScale = new Vector3(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (board.GetComponent<GameBoardController>().currentTpPair != lastTpPulsePair && board.GetComponent<GameBoardController>().currentTpPair == pairID && menu.GetComponent<MenuController>().currentMenuType == -1)
        {
            transform.localScale = new Vector3(0.006f, 0.006f);
        }

        lastTpPulsePair = board.GetComponent<GameBoardController>().currentTpPair;

        if (menu.GetComponent<MenuController>().currentMenuType == -1)
            destScale = 0.005f;
        else
            destScale = 0;

        //go to dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScale - transform.localScale.x) / 5f, transform.localScale.y + (destScale - transform.localScale.y) / 5f, transform.localScale.z);
    }

    public void doParticleEffect()
    {
        particleEffect.GetComponent<ParticleSystem>().Play();
    }
}
