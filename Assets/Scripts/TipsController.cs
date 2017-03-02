using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Project Open Lobotomy 

//light red:    #F79090FF
//light green:  #b5d47fFF
//light blue:   #91C7C7FF
//light purple: #d0a9d6FF
public class TipsController : MonoBehaviour {

    public GameObject menu;
    public GameObject board;
    public int id;
    public Vector3 position;
    string[][] tipListList;
    string[] tipList0 =
    {
        "This  is  <color=#d0a9d6FF>Jimmy</color>\nWe're  here  to  <color=#d0a9d6FF>fix  him</color>",
        "Jimmy  thinks  about  a  lot  of  <color=#d0a9d6FF>different</color>  things  at  once\nThat  is  <i><color=#d0a9d6FF>wrong</color></i>",
        "Jimmy's  jumbled  thoughts  should  <color=#d0a9d6FF>lead</color>  to  <color=#d0a9d6FF>one  coherient  thought</color>\njust  <color=#d0a9d6FF>like  we  were  tought</color>",
        "Jimmy  will  only  listen  to  us  so  much\nso  be  <color=#d0a9d6FF>careful</color>  about  how  you  <color=#d0a9d6FF>fix  him</color>",
        "Always  remember,  this  is\nall  <color=#d0a9d6FF>for  Jimmy</color>",
        "Jimmy  may  be  feeling  <color=#d0a9d6FF>discomfort</color>  now\nbut  this  will  make  him  a  <color=#d0a9d6FF>better  person</color>",
        "Jimmy  <color=#d0a9d6FF>won't  remember</color>  a  thing  when  he  wakes  up\nit's  like  <color=#d0a9d6FF>it  never  happened</color>",
        "Jimmy's  mother  told  us  that  <color=#d0a9d6FF>he  is  defiant</color>\nwe  have  the  power  to  <color=#d0a9d6FF>cure  him</color>",
        "Becuase  Jimmy  is  <color=#d0a9d6FF>mentally  ill</color>\nwe're  going  to  <color=#d0a9d6FF>simplify</color> him",
        "",
        "Jimmy  will  be  <color=#d0a9d6FF>good</color>\nfrom  now  on",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "Jimmy  sometimes  has  <color=#d0a9d6FF>mental  blocks</color>\nwe  just  have  to  <color=#d0a9d6FF>work  around  them</color>",
        "",
        "",
        "",
        "",
        "Jimmy's  <color=#d0a9d6FF>vacant  thoughts</color>  can  be  used  by  us\nin  many  different  ways",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "It  seems  <color=#d0a9d6FF>we  may  have  created  tears</color>  in  Jimmy's  Mind", //teleporters
    };
    string[] tipList1 =
    {
        "Use  <color=#d0a9d6FF>WASD</color>  to  move\nthe  <color=#d0a9d6FF>cursor</color>",
        "",
        "tiles  of  the\n<color=#d0a9d6FF>same  color</color>  combine",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "<color=#d0a9d6FF>Mental  blocks</color>\ncannot  be  moved",
        "",
        "",
        "",
        "",
        "",
    };

    string[] tipList2 =
    {
        "Use  <color=#d0a9d6FF>Arrow  Keys</color>\nto  move  <color=#d0a9d6FF>tiles</color>",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "<color=#d0a9d6FF>Two  vacant  thoughts</color>\nform  a  <color=#d0a9d6FF>mental  block</color>",
    };

    string[] tipList3 =
    {
        "",
        "Tiles  cycle  between\n<color=#F79090FF>Red</color>,  <color=#b5d47fFF>Green</color>,  and  <color=#91C7C7FF>Blue</color>",
        "",
        "complete  each  level\nbefore  the  <color=#d0a9d6FF>bar  runs  out</color>",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "<color=#d0a9d6FF>vacant  thoughts</color>  can  combine\nwith  <color=#d0a9d6FF>any  other  thoughts</color>",
    };

    string[] tipList4 =
    {
        "",
        "Press  <color=#d0a9d6FF>Z</color>  to  <color=#d0a9d6FF>undo</color>\nyour  <color=#d0a9d6FF>last  move</color>"
    };

    float destScale;

    public bool isShowingTip;

	// Use this for initialization
	void Start () {
        transform.position = Camera.main.WorldToScreenPoint(position);
        transform.localScale = new Vector3(0, 0, transform.localScale.z);
        destScale = 0;

        tipListList = new string[5][];
        tipListList[0] = tipList0;
        tipListList[1] = tipList1;
        tipListList[2] = tipList2;
        tipListList[3] = tipList3;
        tipListList[4] = tipList4;
    }

    // Update is called once per frame
    void Update() {

        if (board.GetComponent<GameBoardController>().level - 1 < tipListList[id].Length && board.GetComponent<GameBoardController>().level - 1 >= 0)
            showText(tipListList[id][board.GetComponent<GameBoardController>().level - 1]);
        else
            showText("");

        //hide tip if shouldn't be shown
        if (isShowingTip && board.GetComponent<GameBoardController>().isInGame && menu.GetComponent<MenuController>().currentMenuType == -1)
            destScale = 1f;
        else
            destScale = 0f;

        //go to dest scale
        transform.localScale = new Vector3(transform.localScale.x + (destScale - transform.localScale.x) / 5, transform.localScale.y + (destScale - transform.localScale.y) / 5, 1);

        //reset isShowingTip
        isShowingTip = false;

        //set scale to zero if building board
        if (board.GetComponent<GameBoardController>().isCreatingBoard)
            transform.localScale = new Vector3(0, 0, transform.localScale.z);
	}

    void showText(string newTip)
    {
        GetComponent<Text>().text = newTip;
        isShowingTip = true;
    }
}
