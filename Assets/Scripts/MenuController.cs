using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    public GameObject backButton;
    public GameObject chapterSelector;
    public GameObject levelSelector;

    public GameObject canvas;

    public GameObject board;
    public GameObject menuTile;
    public bool isPaused;
    public int[] numID;
    public int currentID = 0;
    public int currentMenuType = 0;
    public int lastMenu = 0;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            pressedOption();
        }

        if (isPaused && (currentMenuType == 1 || (currentMenuType == 2 && lastMenu == 1)) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                board.GetComponent<GameBoardController>().isUnPausing = true;
                board.GetComponent<GameBoardController>().isAddingBorders = true;
                currentMenuType = -1;
            }
            isPaused = false;
        }
        else if (board.GetComponent<GameBoardController>().isInGame && !board.GetComponent<GameBoardController>().isGoingToWin && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                board.GetComponent<GameBoardController>().isPausing = true;
                board.GetComponent<GameBoardController>().isRemovingTiles = true;
                currentMenuType = 1;
                menuTile.transform.position = new Vector3(menuTile.transform.position.x, numID[currentMenuType] / 2f - 0.5f, menuTile.transform.position.z);
                menuTile.GetComponent<MenuTileController>().destY = numID[currentMenuType] / 2f - 0.5f;
                currentID = 0;
            }
            isPaused = true;
        }
    }

    void switchToMenu(int menuType)
    {
        lastMenu = currentMenuType;
        currentMenuType = menuType;
        menuTile.transform.position = new Vector3(menuTile.transform.position.x, numID[currentMenuType] / 2f - 0.5f, menuTile.transform.position.z);
        menuTile.GetComponent<MenuTileController>().destY = numID[currentMenuType] / 2f - 0.5f;
        currentID = 0;
    }

    void pressedOption()
    {
        if (currentMenuType == 0) //main menu
        {
            if (currentID == 0 && board.GetComponent<GameBoardController>().level > 0) //resume
            {
                board.GetComponent<GameBoardController>().isCreatingBoard = true;
                board.GetComponent<GameBoardController>().aboutToMakeBoard = true;
                board.GetComponent<GameBoardController>().turnsLeft = -1;
                currentMenuType = -1;
                isPaused = false;
            }
            else if (currentID == 1) //new game
            {
                board.GetComponent<GameBoardController>().isCreatingBoard = true;
                board.GetComponent<GameBoardController>().aboutToMakeBoard = true;
                board.GetComponent<GameBoardController>().level = 1;
                board.GetComponent<GameBoardController>().turnsLeft = -1;
                currentMenuType = -1;
                isPaused = false;
            }
            else if (currentID == 2) //level select
            {
                switchToMenu(3);
                GameObject newBackButton = (GameObject)Instantiate(backButton, canvas.transform);
                newBackButton.GetComponent<MenuTextController>().menu = this.gameObject;
                newBackButton.GetComponent<MenuTextController>().type = 3;
                newBackButton.GetComponent<MenuTextController>().setPos();
                newBackButton.GetComponent<LevelSelectController>().tileStartY = menuTile.transform.position.y;
                for (int i = 0; i < 10; i++)
                {
                    GameObject newChapter = (GameObject)Instantiate(chapterSelector, canvas.transform);
                    newChapter.GetComponent<MenuTextController>().menu = this.gameObject;
                    newChapter.GetComponent<Text>().text += (i + 1).ToString();
                    newChapter.GetComponent<MenuTextController>().id = i + 1;
                    newChapter.GetComponent<MenuTextController>().setPos();
                    newChapter.GetComponent<LevelSelectController>().tileStartY = menuTile.transform.position.y;
                }
            }
            else if (currentID == 3) //settings
            {
                switchToMenu(2);
            }
            else if (currentID == 4) //credits
            {
            }
            else if (currentID == 5) //exit
            {
                Application.Quit();
            }
        }
        else if (currentMenuType == 1) //pause menu
        {
            if (currentID == 0) //resume
            {
                if (isPaused)
                {
                    board.GetComponent<GameBoardController>().isUnPausing = true;
                    board.GetComponent<GameBoardController>().isAddingBorders = true;
                    currentMenuType = -1;
                }
                isPaused = false;
            }
            else if (currentID == 1) //settings
            {
                switchToMenu(2);
            }
            else if (currentID == 2) //exit
            {
                switchToMenu(0);
            }
        }
        else if (currentMenuType == 2) //settings menu
        {
            if (currentID == 0) //Back
            {
                switchToMenu(lastMenu);
            }
            else if (currentID == 1) //Music Volume
            {
            }
            else if (currentID == 2) //FX Volume
            {
            }
        }
        else if (currentMenuType == 3) //chapter select
        {
            if (currentID == 0) //back
            {
                GameObject.Destroy(GameObject.FindGameObjectWithTag("BackButton"));
                GameObject[] chapterSelectorsToDestroy = GameObject.FindGameObjectsWithTag("ChapterSelector");
                for (int i = 0; i < chapterSelectorsToDestroy.Length; i++)
                    GameObject.Destroy(chapterSelectorsToDestroy[i]);
                switchToMenu(0);
            }
            else //choose chapter
            {
                GameObject newBackButton = (GameObject)Instantiate(backButton, canvas.transform);
                newBackButton.GetComponent<MenuTextController>().menu = this.gameObject;
                newBackButton.GetComponent<MenuTextController>().type = 3;
                newBackButton.GetComponent<MenuTextController>().setPos();
                for (int i = 0; i < 20; i++)
                {
                    GameObject newLevel = (GameObject)Instantiate(levelSelector, canvas.transform);
                    newLevel.GetComponent<MenuTextController>().menu = this.gameObject;
                    newLevel.GetComponent<Text>().text += (i + 1 + (currentID - 1) * 20).ToString();
                    newLevel.GetComponent<MenuTextController>().id = i +  1;
                    newLevel.GetComponent<MenuTextController>().setPos();
                }

                switchToMenu(4);
            }
        }
        else if (currentMenuType == 4) //level select
        {
            if (currentID == 0) //back
            {
                GameObject.Destroy(GameObject.FindGameObjectWithTag("BackButton"));
                GameObject[] levelSelectorsToDestroy = GameObject.FindGameObjectsWithTag("LevelSelector");
                for (int i = 0; i < levelSelectorsToDestroy.Length; i++)
                    GameObject.Destroy(levelSelectorsToDestroy[i]);
                switchToMenu(3);
            }
            else //choose level
            {

            }
        }
    }
}
