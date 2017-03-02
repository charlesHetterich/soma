using UnityEngine;
using System.Collections;

public class BackgroundMusicController : MonoBehaviour {

    public float pitchDest;
    public GameObject menu;

    // Use this for initialization
    void Start () {
        pitchDest = 1f;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update () {
        GetComponent<AudioSource>().pitch = GetComponent<AudioSource>().pitch + (pitchDest - GetComponent<AudioSource>().pitch) / 10;
        if (menu.GetComponent<MenuController>().currentMenuType == 1 || (menu.GetComponent<MenuController>().currentMenuType == 2 && menu.GetComponent<MenuController>().lastMenu == 1))
        {
            GetComponent<AudioLowPassFilter>().cutoffFrequency += (950 - GetComponent<AudioLowPassFilter>().cutoffFrequency) / 10;
            GetComponent<AudioSource>().volume += (0.7f - GetComponent<AudioSource>().volume) / 10;
        }
        else
        {
            GetComponent<AudioLowPassFilter>().cutoffFrequency += (22000 - GetComponent<AudioLowPassFilter>().cutoffFrequency) / 10;
            GetComponent<AudioSource>().volume += (1f - GetComponent<AudioSource>().volume) / 10;
        }
    }
}
