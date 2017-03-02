using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OwTextController : MonoBehaviour {

    GameObject menu;
    float destScale;
    float originalScale;

    // Use this for initialization
    void Start () {
        menu = GameObject.Find("Menu");

        string[] pains = { "oof", "ow", "ouch" };
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 1);
        GetComponent<Text>().text = pains[Random.Range(0, pains.Length)];
        originalScale = transform.localScale.x;
        transform.localScale = new Vector3(0, 0, transform.localScale.z);
    }

    // Update is called once per frame
    void Update () {
        float destScale = Mathf.Sqrt(transform.parent.transform.position.x * transform.parent.transform.position.x + transform.parent.transform.position.y * transform.parent.transform.position.y) / 2f - 2;
        if (destScale < 0)
            destScale = 0;
        else if (destScale > 1)
            destScale = 1;
        //GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, alpha);

        //destScale = 1f;
        if (menu.GetComponent<MenuController>().currentMenuType != -1)
            destScale = 0;
        transform.localScale = new Vector3(transform.localScale.x + ((originalScale * destScale) - transform.localScale.x) / 5, transform.localScale.y + ((originalScale * destScale) - transform.localScale.y) / 5);

        if (transform.parent.transform.position.y <= -30 || menu.GetComponent<MenuController>().currentMenuType == 0)
        {
            Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
        }
	}
}
