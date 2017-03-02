using UnityEngine;
using System.Collections;

public class MenuTileController : MonoBehaviour {

    public Color[] colorType;
    public int id;

    public float destY;
    public float destScale = 0.5f;
    public bool isHiding = false;
    public GameObject menu;

    public AudioClip tileSlideSound;

    // Use this for initialization
    void Start()
    {
        destY = transform.position.y;

        /*int i = Random.Range(0, colorType.Length);
         id = i;
         GetComponent<SpriteRenderer>().color = colorType[i];*/

        transform.localScale = new Vector3(0, 0, 1);
        transform.position = new Vector3(-1, 2.5f, transform.position.z);
        destY = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.GetComponent<MenuController>().currentMenuType != -1)
        {
            //checks that selector is on tile
            if (transform.position.y == destY)
            {
                if (1 == 1)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        makeMove(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        makeMove(-1);
                    }
                }
            }
        }
        //go to dest location;
        if (transform.position.y < destY)
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        else if (transform.position.y > destY)
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        if (Mathf.Abs(transform.position.y - destY) < 0.21f)
            transform.position = new Vector3(transform.position.x, destY, transform.position.z);

        if (menu.GetComponent<MenuController>().currentMenuType == -1)
            destScale = 0;
        else
            destScale = 0.9f;

        //approach dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScale - transform.localScale.x) / 5, transform.localScale.y + (destScale - transform.localScale.y) / 5, 1);

        //set color to id color
        GetComponent<SpriteRenderer>().color = new Color((GetComponent<SpriteRenderer>().color.r + (colorType[id].r - GetComponent<SpriteRenderer>().color.r) / 10), (GetComponent<SpriteRenderer>().color.g + (colorType[id].g - GetComponent<SpriteRenderer>().color.g) / 10), (GetComponent<SpriteRenderer>().color.b + (colorType[id].b - GetComponent<SpriteRenderer>().color.b) / 10), isHiding ? 0 : 1);
    }

    void makeMove(int y)
    {
        //check if move can be made
        bool canMakeMove = false;
        if (menu.GetComponent<MenuController>().currentID - y >= 0 && menu.GetComponent<MenuController>().currentID - y < menu.GetComponent<MenuController>().numID[menu.GetComponent<MenuController>().currentMenuType])
            canMakeMove = true;

        //make move if we are allowed to
        if (canMakeMove)
        {
            //make sliding noise
            GetComponent<AudioSource>().PlayOneShot(tileSlideSound);

            //move selector and tile
            destY += y;

            //cycle our id
            if (id <= 2)
            {
                id++;
                if (id > 2)
                    id = 0;
            }

            transform.localScale = new Vector3(1.3f, 1.3f, transform.localScale.z);
            menu.GetComponent<MenuController>().currentID -= y;
        }
    }
}
