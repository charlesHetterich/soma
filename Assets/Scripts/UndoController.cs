using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Turn
{
    public GameObject removedTile;
    public GameObject MovedTile;
    public int movedOldID;
    public int removedOldID;
    public float movedX;
    public float movedY;
}

public class UndoController : MonoBehaviour {

    public GameObject owText;

    public AudioClip turnUndoSound;

    public GameObject menu;
    GameObject board;
    public Turn[] turn;
    public int currentTurn;

	// Use this for initialization
	void Start () {
        turn = new Turn[100];
        for (int i = 0; i < 100; i++)
            turn[i] = new Turn();
        board = GameObject.Find("Board");
        currentTurn = 0;
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Z) && currentTurn > 0 && board.GetComponent<GameBoardController>().isInGame && menu.GetComponent<MenuController>().currentMenuType == -1 && !board.GetComponent<GameBoardController>().isGoingToWin)
        {
            //go back to old turn
            currentTurn--;

            //change properties based on old turn
            //make move if we are allowed to
            //make sliding noise

            GetComponent<AudioSource>().PlayOneShot(turnUndoSound);

            //move all teleported tiles if there was a teleportation (tile got to where it was going in this turn
            if (turn[currentTurn].MovedTile.transform.localPosition.x == turn[currentTurn].MovedTile.GetComponent<TileController>().destX && turn[currentTurn].MovedTile.transform.localPosition.y == turn[currentTurn].MovedTile.GetComponent<TileController>().destY)
            {
                GameObject[] liveTiles = GameObject.FindGameObjectsWithTag("Tile");
                for (int i = 0; i < liveTiles.Length; i++)
                {
                    if (!liveTiles[i].GetComponent<TileController>().isHiding)
                        liveTiles[i].GetComponent<TileController>().tpTile();
                }
            }

            turn[currentTurn].MovedTile.GetComponent<TileController>().isUndoingTurn = true;
            turn[currentTurn].MovedTile.GetComponent<TileController>().tileCombiningWith = null;

            //move selector and tile
            turn[currentTurn].MovedTile.GetComponent<TileController>().destX -= turn[currentTurn].movedX;
            turn[currentTurn].MovedTile.GetComponent<TileController>().destY -= turn[currentTurn].movedY;

            //update gameBoardOpenTiles
            board.GetComponent<GameBoardController>().openSpot[(int)(turn[currentTurn].MovedTile.GetComponent<TileController>().destX + 2.5f)][(int)(turn[currentTurn].MovedTile.GetComponent<TileController>().destY + 2.5f)] = true;           //new spot
            board.GetComponent<GameBoardController>().openSpot[(int)(turn[currentTurn].MovedTile.GetComponent<TileController>().destX + 2.5f + turn[currentTurn].movedX)][(int)(turn[currentTurn].MovedTile.GetComponent<TileController>().destY + 2.5f + turn[currentTurn].movedY)] = false;    //old spot

            //decrease number of turns left
            board.GetComponent<GameBoardController>().turnsLeft++;

            turn[currentTurn].MovedTile.transform.localScale = new Vector3(1.3f, 1.3f, turn[currentTurn].MovedTile.transform.localScale.z);
            if (turn[currentTurn].removedTile != null)
                turn[currentTurn].removedTile.transform.localScale = new Vector3(1.3f, 1.3f, turn[currentTurn].removedTile.transform.localScale.z);


            //board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * x * 100, transform.position);
            //board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * y * 100, transform.position);

            //add force to board
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(turn[currentTurn].MovedTile.transform.right * -turn[currentTurn].movedX * 300, turn[currentTurn].MovedTile.transform.position);
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(turn[currentTurn].MovedTile.transform.up * -turn[currentTurn].movedY * 300, turn[currentTurn].MovedTile.transform.position);

            //create ow text
            float x = -turn[currentTurn].movedX;
            float y = -turn[currentTurn].movedY;
            int numOws = Random.Range(1, 3);
            if (board.GetComponent<GameBoardController>().isGoingToWin == true)
                numOws = 5 + Random.Range(0, 5);

            GameObject[] newText = new GameObject[numOws];
            for (int i = 0; i < numOws; i++)
            {
                float scale = 1f + Random.Range(0, 10) / 30f;
                float angle = board.transform.rotation.z + Mathf.Atan2(y, x) + Random.Range(-100, 100) / 100f;
                float force = 500f + Random.Range(1, 500);
                float textColor = 0.75f + Random.Range(0, 100) / 400f;

                newText[i] = (GameObject)Instantiate(owText);
                newText[i].transform.localScale = new Vector3(scale, scale);
                newText[i].GetComponentInChildren<Text>().color = (.5f * new Color(textColor, textColor, textColor, 0)) + .5f * turn[currentTurn].MovedTile.GetComponent<TileController>().colorType[turn[currentTurn].MovedTile.GetComponent<TileController>().id];

                float xPos = 0;
                float yPos = 0;
                float boardAngle = board.transform.rotation.z + Mathf.Atan2(y, x);
                if (x == 1)
                {
                    xPos = turn[currentTurn].MovedTile.transform.position.x + (3.5f - turn[currentTurn].MovedTile.transform.position.x) * Mathf.Cos(boardAngle);
                    yPos = turn[currentTurn].MovedTile.transform.position.y + (3.5f - turn[currentTurn].MovedTile.transform.position.x) * Mathf.Sin(boardAngle);
                }
                if (x == -1)
                {
                    xPos = turn[currentTurn].MovedTile.transform.position.x - (-3.5f - turn[currentTurn].MovedTile.transform.position.x) * Mathf.Cos(boardAngle);
                    yPos = turn[currentTurn].MovedTile.transform.position.y - (-3.5f - turn[currentTurn].MovedTile.transform.position.x) * Mathf.Sin(boardAngle);
                }
                if (y == 1)
                {
                    xPos = turn[currentTurn].MovedTile.transform.position.x + (3.5f - turn[currentTurn].MovedTile.transform.position.y) * Mathf.Cos(boardAngle);
                    yPos = turn[currentTurn].MovedTile.transform.position.y + (3.5f - turn[currentTurn].MovedTile.transform.position.y) * Mathf.Sin(boardAngle);
                }
                if (y == -1)
                {
                    xPos = turn[currentTurn].MovedTile.transform.position.x - (-3.5f - turn[currentTurn].MovedTile.transform.position.y) * Mathf.Cos(boardAngle);
                    yPos = turn[currentTurn].MovedTile.transform.position.y - (-3.5f - turn[currentTurn].MovedTile.transform.position.y) * Mathf.Sin(boardAngle);
                }
                newText[i].transform.position = new Vector3(xPos, yPos);

                if (turn[currentTurn].movedX != 0)//text comes from left or righ side (because the screen is wider than it is tall)
                    newText[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle) * 2, force * Mathf.Sin(angle)));
                else
                    newText[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle), force * Mathf.Sin(angle)));
                newText[i].GetComponent<Rigidbody2D>().AddTorque(Random.Range(0, 500));

                board.GetComponent<GameBoardController>().addPain(newText[i]);
            }

            //change id
            turn[currentTurn].MovedTile.GetComponent<TileController>().id = turn[currentTurn].movedOldID;
            if (turn[currentTurn].removedTile != null)
            {
                turn[currentTurn].removedTile.GetComponent<TileController>().id = turn[currentTurn].removedOldID;
                turn[currentTurn].removedTile.GetComponent<TileController>().isHiding = false;
            }
        }
	}
}
