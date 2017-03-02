using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class TextController : MonoBehaviour {

    public Text scoreText;
    public GameBoardController gameBoardWatching;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = gameBoardWatching.turnsLeft.ToString();
	}
}
