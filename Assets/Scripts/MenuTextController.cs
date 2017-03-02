using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TYPE {
    MENU,
    PAUSE,
    SETTING,
}

public class MenuTextController : MonoBehaviour {

    public GameObject menu;

    public int id;
    public int type;
    public float destScale = 1f;

    float destUnselectedX;
    float destSelectedX;
    float destX;

	// Use this for initialization
	void Start (){
        Vector3 textDimensions = Camera.main.ScreenToWorldPoint(new Vector3(transform.localScale.x, transform.localScale.y));
        Vector3 textPosition = new Vector3();
        
        textPosition = Camera.main.WorldToScreenPoint(new Vector3(0, (menu.GetComponent<MenuController>().numID[type] / 2f - 0.5f) - id));

        transform.position = textPosition;
        transform.localScale = new Vector3(0, 0, transform.localScale.z);

        //set dest x locations
        destUnselectedX = textPosition.x;
        destSelectedX = textPosition.x + (Camera.main.WorldToScreenPoint(new Vector3(1f, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0)).x);
    }
	
	// Update is called once per frame
	void Update () {
        if (menu.GetComponent<MenuController>().currentMenuType == type)
        {
            destScale = 1f;
            destX = destUnselectedX;
            if (menu.GetComponent<MenuController>().currentID == id)
                destX = destSelectedX;
        }
        else
        {
            destScale = 0f;
            destX = destUnselectedX;
        }

        //faded out 'resume' start menu button if first time in game (ie. board.level == 0)
        if (id == 0 && type == 0)
        {
            if (menu.GetComponent<MenuController>().board.GetComponent<GameBoardController>().level <= 0)
                GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0.5f);
            else
                GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 1f);
        }
        else if ((id == 4 && type == 0) || (id == 1 && type == 2) || (id == 2 && type == 2)) //or it is credits, music volume, or fx volume, since these are not yet added
            GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0.5f);

        //go to dest x
        transform.position = new Vector3(transform.position.x + (destX - transform.position.x) / 5, transform.position.y, transform.position.z);

        //go to dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScale - transform.localScale.x) / 5, transform.localScale.y + (destScale - transform.localScale.y) / 5);
    }

    // Use this for initialization
    public void setPos()
    {
        Vector3 textDimensions = Camera.main.ScreenToWorldPoint(new Vector3(transform.localScale.x, transform.localScale.y));
        Vector3 textPosition = new Vector3();

        textPosition = Camera.main.WorldToScreenPoint(new Vector3(0, (menu.GetComponent<MenuController>().numID[type] / 2f - 0.5f) - id));

        transform.position = textPosition;
        transform.localScale = new Vector3(0, 0, transform.localScale.z);

        //set dest x locations
        destUnselectedX = textPosition.x;
        destSelectedX = textPosition.x + (Camera.main.WorldToScreenPoint(new Vector3(1f, 0)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0)).x);
    }
}
