using UnityEngine;
using System.Collections;

public class BorderController : MonoBehaviour {

    GameObject board;
    public float destScale;
    public float scale;
    public bool isVert;

	// Use this for initialization
	void Start () {
        board = GameObject.Find("Board");
        transform.parent = board.transform;
    }
	
	// Update is called once per frame
	void Update () {

        scale += (destScale - scale) / 10;

        if (isVert)
            transform.localScale = new Vector3(transform.localScale.x, scale);
        else
            transform.localScale = new Vector3(scale, transform.localScale.y);
	}
}
