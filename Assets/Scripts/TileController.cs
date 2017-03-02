using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileController : MonoBehaviour {

    public GameObject owText;
    GameObject canvas;
    public GameObject undoButton;
    public Color[] colorType;
    public int id;
    GameObject selector;
    public GameObject board;
    GameObject menu;
    public GameObject tileCombiningWith;

    public bool isUndoingTurn;

    public float destX;
    public float destY;
    public float destScaleX = 0.9f;
    public float destScaleY = 0.9f;
    public bool shouldDie = false;
    bool justGotToDest = false;
    public bool isHiding = false;

    bool justSpawned = true;

    public AudioClip tileSlideSound;

    // Use this for initialization
    void Start () {
        justSpawned = true;
        board = GameObject.Find("Board");
        selector = GameObject.Find("Selector");
        undoButton = GameObject.Find("UndoButton");
        menu = GameObject.Find("Menu");
        canvas = GameObject.Find("Canvas");


        transform.parent = board.transform;


        destX = transform.localPosition.x;
        destY = transform.localPosition.y;

        transform.localScale = new  Vector3(0, 0, 1);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if selector is on this tile and this tile is white, make selector black so it is visible
        if (transform.localPosition.x == selector.transform.localPosition.x && transform.localPosition.y == selector.transform.localPosition.y)
            if (id == 4)
                selector.GetComponent<SelectorController>().destColor = new Color(0, 0, 0, 1f);

        if (!board.GetComponent<GameBoardController>().isLoadingLevel && menu.GetComponent<MenuController>().currentMenuType == -1)
        {
            //checks that selector is on tile
            if (transform.localPosition.x == destX && transform.localPosition.y == destY)
            {
                if (transform.localPosition.x == selector.transform.localPosition.x && transform.localPosition.y == selector.transform.localPosition.y && !isHiding)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        makeMove(0, 1);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        makeMove(0, -1);
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        makeMove(-1, 0);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        makeMove(1, 0);
                    }
                }
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

        //when tile moved first gets to its destination
        if (transform.localPosition.x == destX && transform.localPosition.y == destY && !justGotToDest)
        {
            print("a");

            board.GetComponent<GameBoardController>().shouldCheckForComplete = true;
            justGotToDest = true;

            if (!isUndoingTurn)
                board.GetComponent<GameBoardController>().shouldTpAllTiles = true;
            isUndoingTurn = false;

            if (justSpawned)
                board.GetComponent<GameBoardController>().shouldTpAllTiles = false;
        }

        //if tiles should tp, try to tp this one
        if (board.GetComponent<GameBoardController>().isTpingAllTiles && !isHiding && board.GetComponent<GameBoardController>().isInGame)
            tpTile();

        if (!(transform.localPosition.x == destX && transform.localPosition.y == destY && justGotToDest))
            justGotToDest = false;

        if (tileCombiningWith != null && transform.localPosition.x == destX && transform.localPosition.y == destY)
        {
            tileCombiningWith.GetComponent<TileController>().isHiding = true;
            tileCombiningWith = null;
        }
        //if dying we should shrink
        if (shouldDie)
        {
            destScaleX = -1f;
            destScaleY = -1f;
        }
        //approach dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScaleX - transform.localScale.x) / 10, transform.localScale.y + (destScaleY - transform.localScale.y) / 10, 1);

        //if shrunk and dead, really die
        if (shouldDie && transform.localScale.x <= 0)
            GameObject.Destroy(this.gameObject);

        //set color to id color
        GetComponent<SpriteRenderer>().color = new Color((GetComponent<SpriteRenderer>().color.r + (colorType[id].r - GetComponent<SpriteRenderer>().color.r) / 10), (GetComponent<SpriteRenderer>().color.g + (colorType[id].g - GetComponent<SpriteRenderer>().color.g) / 10), (GetComponent<SpriteRenderer>().color.b + (colorType[id].b - GetComponent<SpriteRenderer>().color.b) / 10), isHiding? 0 : 1);

        //has been spawned for a full frame update cycle now
        justSpawned = false;
    }

    void makeMove(int x, int y)
    {
        //check if move can be made
        bool canMakeMove = true;
        bool isCombining = false;

        //set it to false if no turns left
        canMakeMove = board.GetComponent<GameBoardController>().turnsLeft > 0 && id != 3;

        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < allTiles.Length && canMakeMove && !isCombining; i++)
        {
            if (!allTiles[i].GetComponent<TileController>().isHiding)
            {
                //if a tile is where we want to go, we cant make the move...
                if (destX + x == allTiles[i].GetComponent<TileController>().destX && destY + y == allTiles[i].GetComponent<TileController>().destY)
                    canMakeMove = false;

                //... unless the move is into the same color/white
                if (!canMakeMove && (allTiles[i].GetComponent<TileController>().id == id || allTiles[i].GetComponent<TileController>().id == 4 || id == 4) && !(allTiles[i].GetComponent<TileController>().id == 3 || id == 3))
                {
                    isCombining = true;
                    tileCombiningWith = allTiles[i];
                    board.GetComponent<GameBoardController>().score++;
                }

                //if going to same color, we can make the move
                if (isCombining)
                    canMakeMove = true;

                //if going off the map
                if (destX + x + 2.5f < 0 || destX + x + 2.5f > 5 || destY + y + 2.5f < 0 || destY + y + 2.5f > 5)
                    canMakeMove = false;
            }
        }

        //make move if we are allowed to
        if (canMakeMove)
        {
            //if was in the middle of undoing turn, not undoing anymore
            //isUndoingTurn = false;

            //if this is the winning move, let the board know
            if (isCombining && (board.GetComponent<GameBoardController>().liveTileCount <= 2 || (id == 4 && tileCombiningWith.GetComponent<TileController>().id == 4 && board.GetComponent<GameBoardController>().liveTileCount <= 3)))
                board.GetComponent<GameBoardController>().isGoingToWin = true;

            //set old localPositions before making this turn
            undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].MovedTile = this.gameObject;
            undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].movedX = x;
            undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].movedY = y;
            undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].movedOldID = id;
            if (tileCombiningWith != null)
            {
                undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].removedTile = tileCombiningWith;
                undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].removedOldID = tileCombiningWith.GetComponent<TileController>().id;
            }
            else
                undoButton.GetComponent<UndoController>().turn[undoButton.GetComponent<UndoController>().currentTurn].removedTile = null;

            //increase turn
            undoButton.GetComponent<UndoController>().currentTurn++;

            //make sliding noise
            GetComponent<AudioSource>().PlayOneShot(tileSlideSound);

            //move selector and tile
            destX += x;
            destY += y;
            selector.GetComponent<SelectorController>().destX += x;
            selector.GetComponent<SelectorController>().destY += y;

            //update gameBoardOpenTiles
            board.GetComponent<GameBoardController>().openSpot[(int)(destX + 2.5f)][(int)(destY + 2.5f)] = false;           //new spot
            board.GetComponent<GameBoardController>().openSpot[(int)(destX + 2.5f - x)][(int)(destY + 2.5f - y)] = true;    //old spot

            //decrease number of turns left
            board.GetComponent<GameBoardController>().turnsLeft--;

            //add force to board
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * x * 300, transform.position);
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * y * 300, transform.position);
            if (board.GetComponent<GameBoardController>().isGoingToWin == true)
            {
                board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * x * 1000, transform.position);
                board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * y * 1000, transform.position);
            }

            //create ow text
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
                newText[i].GetComponentInChildren<Text>().color = (.5f * new Color(textColor, textColor, textColor, 0)) + .5f * colorType[id];

                float xPos = 0;
                float yPos = 0;
                float boardAngle = board.transform.rotation.z + Mathf.Atan2(y, x);
                if (x == 1)
                {
                    xPos = transform.position.x + (3.5f - transform.position.x) * Mathf.Cos(boardAngle);
                    yPos = transform.position.y + (3.5f - transform.position.x) * Mathf.Sin(boardAngle);
                }
                if (x == -1)
                {
                    xPos = transform.position.x - (-3.5f - transform.position.x) * Mathf.Cos(boardAngle);
                    yPos = transform.position.y - (-3.5f - transform.position.x) * Mathf.Sin(boardAngle);
                }
                if (y == 1)
                {
                    xPos = transform.position.x + (3.5f - transform.position.y) * Mathf.Cos(boardAngle);
                    yPos = transform.position.y + (3.5f - transform.position.y) * Mathf.Sin(boardAngle);
                }
                if (y == -1)
                {
                    xPos = transform.position.x - (-3.5f - transform.position.y) * Mathf.Cos(boardAngle);
                    yPos = transform.position.y - (-3.5f - transform.position.y) * Mathf.Sin(boardAngle);
                }
                newText[i].transform.position = new Vector3(xPos, yPos);

                if (x != 0)//text comes from left or righ side (because the screen is wider than it is tall)
                    newText[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle) * 2, force * Mathf.Sin(angle)));
                else
                    newText[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle), force * Mathf.Sin(angle)));
                newText[i].GetComponent<Rigidbody2D>().AddTorque(Random.Range(0, 500));

                if (board.GetComponent<GameBoardController>().isGoingToWin == true)
                {
                    newText[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle), force * Mathf.Sin(angle)));
                    board.GetComponent<AudioSource>().PlayOneShot(board.GetComponent<GameBoardController>().symbolCrash[Random.Range(0, board.GetComponent<GameBoardController>().symbolCrash.Length)]);
                }

                board.GetComponent<GameBoardController>().addPain(newText[i]);
            }

            //cycle our id
            if (id <= 2)
            {
                id++;
                if (id > 2)
                    id = 0;
            }

            if (isCombining && id == 4 && tileCombiningWith.GetComponent<TileController>().id == 4)
                id = 3;

            transform.localScale = new Vector3(1.3f, 1.3f, transform.localScale.z);
            //set other tiles id to ours
            if (tileCombiningWith != null)
            {
                if (id == 4)
                {
                    id = tileCombiningWith.GetComponent<TileController>().id;
                    if (id <= 2)
                    {
                        id++;
                        if (id > 2)
                            id = 0;
                    }
                }

                tileCombiningWith.GetComponent<TileController>().id = id;
                tileCombiningWith.transform.localScale = new Vector3(1.3f, 1.3f, tileCombiningWith.transform.localScale.z);
            }
        }
        else
        {
            //add force to board
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * x * 100, transform.position);
            board.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.up * y * 100, transform.position);
        }
    }

    public void tpTile()
    {
        GameObject[] allTeleporters = GameObject.FindGameObjectsWithTag("Teleporter");
        bool didTp = false;
        for (int i = 0; i < allTeleporters.Length && !didTp; i++)
        {
            if (transform.localPosition.x == allTeleporters[i].transform.localPosition.x && transform.localPosition.y == allTeleporters[i].transform.localPosition.y)
            {
                //teleport tile
                transform.localPosition = new Vector3(allTeleporters[i].GetComponent<TeleporterController>().pairTeleporter.transform.localPosition.x, allTeleporters[i].GetComponent<TeleporterController>().pairTeleporter.transform.localPosition.y, transform.localPosition.z);
                destX = transform.localPosition.x;
                destY = transform.localPosition.y;
                didTp = true;

                //animate teleporter
                allTeleporters[i].transform.localScale = new Vector3(0.015f, 0.015f);
                allTeleporters[i].GetComponent<TeleporterController>().pairTeleporter.transform.localScale = new Vector3(0.015f, 0.015f);
                allTeleporters[i].GetComponent<TeleporterController>().doParticleEffect();
                allTeleporters[i].GetComponent<TeleporterController>().pairTeleporter.GetComponent<TeleporterController>().doParticleEffect();

                //let board know to play tp sound
                board.GetComponent<GameBoardController>().tileDidTp = true;
            }
        }
    }
}
