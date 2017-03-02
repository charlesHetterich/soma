using UnityEngine;
using System.Collections;

[System.Serializable]

public class GameBoardController : MonoBehaviour {

    int newPainPosition = 0;
    public GameObject[] painsHeldUp;

    public GameObject undoController;
    public GameObject vertBar;
    public GameObject horzBar;
    public GameObject Tile;
    public GameObject selector;
    public bool shouldAddTile = false;
    public bool[][] openSpot = new bool[6][];
    public int score = 1;
    public int numLevels = 6;
    public int level = 1;
    public int turnsLeft = 1;
    public int[] maxTurns;

    //start menu transition (draw the board)
    GameObject[] borders;
    public bool aboutToMakeBoard;
    public bool isMakingBoard;
    public bool isCreatingBoard;
    float boardLoadTimer = 0f;
    float borderAppearDelay = 0.1f;
    int currentBorder = 0;

    //pause menu transition
    float pauseTransitionTimer = 0f;
    float timeMiddleOfRedrawBoard = 0.2f;
    public bool isWaitingPauseTransition;
    //pausing
    public bool isPausing;
    public bool isRemovingTiles;
    public bool isRemovingBorders;
    //unpausing
    public bool isUnPausing;
    public bool isAddingBorders;
    public bool isAddingTiles;

    //in game
    public bool shouldCheckForComplete = false;
    bool didLose = false;
    public bool isInGame = false;
    public bool isGoingToWin = false;
    public int liveTileCount = 0;
    public AudioClip[] symbolCrash;

    public float destX = 0;
    public float destY = 0;
    public float destRotation = 0;

    public bool moveJustCompleted = false;

    public GameObject tpSoundSource;
    public bool tileDidTp = false;
    public bool shouldTpAllTiles = false;
    public bool isTpingAllTiles = false;
    public int numTpPairs;
    public float timeBetweenPulse = 0.5f;
    float pulseTimer = 0;
    public int currentTpPair = 0;

    //loading level
    public bool isLoadingLevel = true;
    public bool levelJustDone = false;
    public bool killingFinalTile = false;
    public bool levelJustBegun = false;
    public bool isCreatingLevel = false;
    public bool levelJustLoaded = false;
    float levelLoadingTimer = 0.0f;
    float levelJustDoneDelay = 0.2f;
    float levelJustBegunDelay = 1.0f;
    float tileAppearDelay = 0.1f;
    int levelLoadX = 0;
    int levelLoadY = 0;
    float levelJustLoadedDelay = 0.5f;
    Color32[] levelPixels;

    public Color32[] colorID;

    // Use this for initialization
    void Start ()
    {
        levelPixels = new Color32[36];

        openSpot[0] = new bool[6];
        openSpot[1] = new bool[6];
        openSpot[2] = new bool[6];
        openSpot[3] = new bool[6];
        openSpot[4] = new bool[6];
        openSpot[5] = new bool[6];

        for (int i = 0; i < openSpot.Length; i++)
        {
            for (int j = 0; j < openSpot[i].Length; j++)
            {
                openSpot[i][j] = true;
            }
        }

        //vert
        for (int i = 0; i < 5; i++)
        {
            GameObject newBar = (GameObject)Instantiate(vertBar, new Vector3(-2f + (float)i + transform.position.x, 0f + transform.position.y, 0f), Quaternion.identity);
            newBar.GetComponent<BorderController>().destScale = 0;
            newBar.GetComponent<BorderController>().scale = 0;
        }

        //horz
        for (int i = 0; i < 5; i++)
        {
            GameObject newBar = (GameObject)Instantiate(horzBar, new Vector3(0f + transform.position.x, -2f + (float)i + transform.position.y, 0f), Quaternion.identity);
            newBar.GetComponent<BorderController>().destScale = 0;
            newBar.GetComponent<BorderController>().scale = 0;
        }
        borders = GameObject.FindGameObjectsWithTag("Border");

        turnsLeft = maxTurns[level];

        painsHeldUp = new GameObject[65];


	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isPausing)
        {
            pauseTransitionTimer += Time.deltaTime;
            if (isRemovingTiles)
            {
                GameObject[] liveTiles = GameObject.FindGameObjectsWithTag("Tile");
                for (int i = 0; i < liveTiles.Length; i++)
                {
                    liveTiles[i].GetComponent<TileController>().destScaleX = 0f;
                    liveTiles[i].GetComponent<TileController>().destScaleY = 0f;
                }

                selector.GetComponent<SelectorController>().destScaleX = 0f;
                selector.GetComponent<SelectorController>().destScaleY = 0f;

                isRemovingTiles = false;
                isWaitingPauseTransition = true;
                pauseTransitionTimer = 0f;

            }
            else if (isWaitingPauseTransition)
            {
                if (pauseTransitionTimer >= timeMiddleOfRedrawBoard)
                {
                    isWaitingPauseTransition = false;
                    isRemovingBorders = true;
                }
            }
            else if (isRemovingBorders)
            {
                for (int i = 0; i < borders.Length; i++)
                    borders[i].GetComponent<BorderController>().destScale = 0f;

                isRemovingBorders = false;
                isPausing = false;
            }
        }
        if (isUnPausing)
        {
            pauseTransitionTimer += Time.deltaTime;
            if (isAddingBorders)
            {
                for (int i = 0; i < borders.Length; i++)
                    borders[i].GetComponent<BorderController>().destScale = 6f;

                isAddingBorders = false;
                isWaitingPauseTransition = true;
                pauseTransitionTimer = 0f;
            }
            else if (isWaitingPauseTransition)
            {
                if (pauseTransitionTimer >= timeMiddleOfRedrawBoard)
                {
                    isWaitingPauseTransition = false;
                    isAddingTiles = true;
                }
            }
            else if (isAddingTiles)
            {
                GameObject[] liveTiles = GameObject.FindGameObjectsWithTag("Tile");
                for (int i = 0; i < liveTiles.Length; i++)
                {
                    liveTiles[i].GetComponent<TileController>().destScaleX = 0.9f;
                    liveTiles[i].GetComponent<TileController>().destScaleY = 0.9f;
                }

                selector.GetComponent<SelectorController>().destScaleX = 0.1f;
                selector.GetComponent<SelectorController>().destScaleY = 0.1f;

                isAddingTiles = false;
                isUnPausing = false;
            }
        }
        if (isCreatingBoard)
        {
            boardLoadTimer += Time.deltaTime;

            if (aboutToMakeBoard)
            {
                boardLoadTimer = 0f;
                aboutToMakeBoard = false;
                isMakingBoard = true;

            }
            else if (isMakingBoard)
            {
                //set new board dest
                if (boardLoadTimer >= borderAppearDelay)
                {
                    boardLoadTimer = 0f;
                    for (int i = 0; i < borders.Length; i++)
                        borders[i].GetComponent<BorderController>().destScale = 6f;

                    selector.GetComponent<SelectorController>().destScaleX = 0.1f;
                    selector.GetComponent<SelectorController>().destScaleY = 0.1f;

                    if (level > 0)
                        didLose = true;
                    shouldCheckForComplete = false;
                    isInGame = false;

                    isMakingBoard = false;
                    isCreatingBoard = false;
                    isLoadingLevel = true;
                    levelJustDone = true;
                    levelLoadingTimer = 0f;
                }
            }
        }
        if (isLoadingLevel)
        {
            levelLoadingTimer += Time.deltaTime;

            if (levelJustDone)
            {
                if (levelLoadingTimer >= levelJustDoneDelay)
                {
                    GameObject[] allPains = GameObject.FindGameObjectsWithTag("PainText");
                    for (int i = 0; i < allPains.Length; i++)
                    {
                        allPains[i].GetComponent<Rigidbody2D>().gravityScale = 1;
                        allPains[i].GetComponent<Rigidbody2D>().drag = 0.15f;
                        allPains[i].GetComponent<Rigidbody2D>().angularDrag = 0.15f;
                    }

                    levelLoadingTimer = 0.0f;
                    levelJustDone = false;
                    killingFinalTile = true;
                }
            }
            else if (killingFinalTile)
            {
                GameObject[] liveTiles = GameObject.FindGameObjectsWithTag("Tile");
                for (int i = 0; i < liveTiles.Length; i++)
                    liveTiles[i].GetComponent<TileController>().shouldDie = true;

                //if (GameObject.FindGameObjectsWithTag("Tile").Length != 0)
                //    GameObject.FindGameObjectWithTag("Tile").GetComponent<TileController>().shouldDie = true;
                if (GameObject.FindGameObjectsWithTag("Tile").Length == 0)
                {
                    levelLoadingTimer = 0.0f;
                    killingFinalTile = false;
                    levelJustBegun = true;
                }
            }
            else if (levelJustBegun)
            {
                if (levelLoadingTimer >= levelJustBegunDelay)
                {
                    levelLoadingTimer = 0.0f;
                    levelJustBegun = false;
                    isCreatingLevel = true;
                    if (!didLose)
                    {
                        level++;
                        if (level > numLevels)
                            level = 1;
                    }
                    didLose = false;
                    turnsLeft = maxTurns[level - 1];
                    loadLevelImage();
                    levelLoadX = 0;
                    levelLoadY = 0;
                    undoController.GetComponent<UndoController>().currentTurn = 0;
                    isGoingToWin = false;
                }
            }
            else if (isCreatingLevel)
            {
                if (levelLoadingTimer >= tileAppearDelay)
                {
                    if (levelPixels[levelLoadY * 6 + levelLoadX].a != 0)
                        createTile(levelLoadX, levelLoadY);
                    levelLoadX++;
                    if (levelLoadX >= 6)
                    {
                        levelLoadX = 0;
                        levelLoadY++;
                    }
                    while ((levelLoadY * 6 + levelLoadX) < levelPixels.Length && levelPixels[levelLoadY * 6 + levelLoadX].a == 0)
                    {
                        levelLoadX++;
                        if (levelLoadX >= 6)
                        {
                            levelLoadX = 0;
                            levelLoadY++;
                        }
                    }

                    levelLoadingTimer = 0.0f;
                }

                if ((levelLoadY * 6 + levelLoadX) >= levelPixels.Length)
                {
                    levelLoadingTimer = 0.0f;
                    isCreatingLevel = false;
                    levelJustLoaded = true;
                }
            }
            else if (levelJustLoaded)
            {
                if (levelLoadingTimer >= levelJustLoadedDelay)
                {
                    levelLoadingTimer = 0.0f;
                    levelJustLoaded = false;
                    isLoadingLevel = false;
                    isInGame = true;
                }
            }
        }
        if (isInGame)
        {
            //pulse teleporters
            if (timeBetweenPulse + pulseTimer < Time.time)
            {
                currentTpPair++;
                if (currentTpPair >= numTpPairs)
                    currentTpPair = 0;
                pulseTimer = Time.time;
            }

            //check if level is complete
            if (shouldCheckForComplete)
            {
                GameObject[] currentTiles = GameObject.FindGameObjectsWithTag("Tile");
                liveTileCount = 0;
                for (int i = 0; i < currentTiles.Length; i++)
                    if (currentTiles[i].GetComponent<TileController>().id != 3 && !currentTiles[i].GetComponent<TileController>().isHiding)
                        liveTileCount++;

                if (liveTileCount <= 1)
                {
                    levelLoadingTimer = 0.0f;
                    isLoadingLevel = true;
                    levelJustDone = true;
                    isInGame = false;
                }
            }
            shouldCheckForComplete = false;

            isTpingAllTiles = false;
            if (shouldTpAllTiles)
                isTpingAllTiles = true;
            shouldTpAllTiles = false;

            if (tileDidTp)
                tpSoundSource.GetComponent<AudioSource>().Play();
            tileDidTp = false;
        }

        //go to dest location if not doing physics
        transform.position = new Vector3(transform.position.x + (destX - transform.position.x) / 10, transform.position.y + (destY - transform.position.y) / 10);
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + (destRotation - transform.rotation.z) / 10, transform.rotation.w);

        if (shouldAddTile == true)
        {
            for (bool spotChosen = false; !spotChosen;)
            {
                int x = Random.Range(0, openSpot.Length);
                int y = Random.Range(0, openSpot[0].Length);

                if (openSpot[x][y])
                {
                    GameObject newTile = (GameObject)Instantiate(Tile, new Vector3(-2.5f + (float)x, -2.5f + (float)y, -1f), Quaternion.identity);
                    openSpot[x][y] = false;
                    spotChosen = true;
                }
            }
            shouldAddTile = false;
        }
    }

    void EmptyMap()
    {
    }

    void loadLevelImage()
    {
        EmptyMap();

        // Read the image data from the file in StreamingAssets
        string filePath = Application.streamingAssetsPath + "/levels/lvl_" + level.ToString() + ".png";
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D levelMap = new Texture2D(2, 2);
        levelMap.LoadImage(bytes);


        // Get the raw pixels from the level imagemap
        levelPixels = levelMap.GetPixels32();
    }

    void createTile(int x, int y)
    {
        bool foundColor = false;
        for (int i = 0; i < colorID.Length && !foundColor; i++)
        {
            if (levelPixels[y * 6 + x].Equals(colorID[i]))
            {
                //new Vector3(-2.5f + (float)x + transform.position.x, -2.5f + (float)y + transform.position.y, -1f)
                GameObject newTile = (GameObject)Instantiate(Tile, transform);
                newTile.transform.localRotation = Quaternion.identity;
                newTile.transform.localPosition = new Vector3(-2.5f + (float)x, -2.5f + (float)y, -1f);
                openSpot[x][y] = false;
                newTile.GetComponent<TileController>().id = i;
                foundColor = true;
            }
        }
    }

    public void addPain(GameObject newPain)
    {
        if (painsHeldUp[newPainPosition] != null)
        {
            painsHeldUp[newPainPosition].GetComponent<Rigidbody2D>().gravityScale = 1;
            painsHeldUp[newPainPosition].GetComponent<Rigidbody2D>().drag = 0.15f;
            painsHeldUp[newPainPosition].GetComponent<Rigidbody2D>().angularDrag = 0.15f;
        }
        painsHeldUp[newPainPosition] = newPain;

        newPainPosition++;
        if (newPainPosition >= painsHeldUp.Length)
            newPainPosition = 0;
    }
}
