using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

    public bool isPaused;

    public GameObject backgroundMusic;

    public GameObject settingsButton;

    float maxScaleX;
    float screenWidthProportion;
    float properX;

    // Use this for initialization
    void Start () {
        isPaused = false;

        //find proper x position for everything
        maxScaleX = 13.32f;
        print("screen width: " + Screen.width.ToString());
        float baseProportion = 1024f / 768f;
        float screenProportion = (float)Screen.width / (float)Screen.height;
        screenWidthProportion = screenProportion / baseProportion;
        properX = -(maxScaleX * screenWidthProportion - 1);

        settingsButton.transform.position = new Vector3(properX, settingsButton.transform.position.y, settingsButton.transform.position.z);

        //transform.localScale = new Vector3(maxScaleX * screenWidthProportion, transform.localScale.y, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;


        if (isPaused)
        {
            backgroundMusic.GetComponent<BackgroundMusicController>().pitchDest = 0;
        }
        else
        {
            backgroundMusic.GetComponent<BackgroundMusicController>().pitchDest = 1;
        }
    }
}
