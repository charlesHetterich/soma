using UnityEngine;
using System.Collections;

public class LevelSelectController : MonoBehaviour {

    public float tileStartY = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 textPosition = new Vector3();

        //textPosition = Camera.main.WorldToScreenPoint(new Vector3(0, (GetComponent<MenuTextController>().menu.GetComponent<MenuController>().numID[GetComponent<MenuTextController>().type] / 2f - 0.5f) - GetComponent<MenuTextController>().id + ((tileStartY - GetComponent<MenuTextController>().menu.GetComponent<MenuController>().menuTile.transform.position.y) / 1.5f)));

        //transform.position = new Vector3(transform.position.x, textPosition.y, transform.position.z);
    }
}
