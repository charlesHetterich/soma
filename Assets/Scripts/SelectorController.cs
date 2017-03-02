using UnityEngine;
using System.Collections;

[System.Serializable]
public class SelectorController : MonoBehaviour {

    public float destX;
    public float destY;
    public float destScaleX;
    public float destScaleY;
    public Color destColor;
    public GameObject board;

    float lastX;
    float lastY;

    public GameObject menu;

    // Use this for initialization
    void Start () {

        destX = transform.localPosition.x;
        destY = transform.localPosition.y;
        lastX = transform.localPosition.x;
        lastY = transform.localPosition.y;
        destScaleX = 0;
        destScaleY = 0;

        transform.localScale = new Vector3(0, 0);

        GameBoardController boardController = GameObject.Find("Board").GetComponent<GameBoardController>();
    }

    // Update is called once per frame
    void Update ()
    {
        //go to dest color (white unless on white tile)
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r + (destColor.r - GetComponent<SpriteRenderer>().color.r) / 10, GetComponent<SpriteRenderer>().color.g + (destColor.g - GetComponent<SpriteRenderer>().color.g) / 10, GetComponent<SpriteRenderer>().color.b + (destColor.b - GetComponent<SpriteRenderer>().color.b) / 10, GetComponent<SpriteRenderer>().color.a + (destColor.a - GetComponent<SpriteRenderer>().color.a) / 10);
        destColor = new Color(1f, 1f, 1f, 1f);

        //take movement input if at destination location
        if (transform.localPosition.x == destX && transform.localPosition.y == destY && menu.GetComponent<MenuController>().currentMenuType == -1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (destY + 1 + 2.5f < 6)
                    destY++;
                else
                    board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * 100, transform.position);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (destY - 1 + 2.5f >= 0)
                    destY--;
                else
                    board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * -100, transform.position);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (destX - 1 + 2.5f >= 0)
                    destX--;
                else
                    board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * -100, transform.position);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (destX + 1 + 2.5f < 6)
                    destX++;
                else
                    board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * 100, transform.position);
            }
        }

        //go to dest location;
        if (transform.localPosition.x < destX)
            transform.localPosition = new Vector3(transform.localPosition.x + 0.2f, transform.localPosition.y, transform.localPosition.z);
        else if (transform.localPosition.x > destX)
            transform.localPosition = new Vector3(transform.localPosition.x - 0.2f, transform.localPosition.y, transform.localPosition.z);
        if (Mathf.Abs(transform.localPosition.x - destX) < 0.21f)
            transform.localPosition = new Vector3(destX, transform.localPosition.y, transform.localPosition.z);

        if (transform.localPosition.y < destY)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.2f, transform.localPosition.z);
        else if (transform.localPosition.y > destY)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.2f, transform.localPosition.z);
        if (Mathf.Abs(transform.localPosition.y - destY) < 0.21f)
            transform.localPosition = new Vector3(transform.localPosition.x, destY, transform.localPosition.z);

        if (menu.GetComponent<MenuController>().currentMenuType == -1)
        {

            destScaleX = 0.1f - Mathf.Abs(transform.localPosition.y - lastY) / 4 + Mathf.Abs(transform.localPosition.x - lastX) / 2;
            destScaleY = 0.1f - Mathf.Abs(transform.localPosition.x - lastX) / 4 + Mathf.Abs(transform.localPosition.y - lastY) / 2;
        }
        lastX = transform.localPosition.x;
        lastY = transform.localPosition.y;

        //set dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScaleX - transform.localScale.x) / 5, transform.localScale.y + (destScaleY - transform.localScale.y) / 5);
    }
}
