using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct Player
{
    public int winCount;
    public float resultDistance;
}

public class UIScript : MonoBehaviour
{

    public GameObject ball;
    public Button reset;
    public GameObject[] jackStar;
    public GameObject[] finnStar;
    public GameObject arrow;
    public InputField inputForce;
    public Text resultTitle;
    public Text showGauge;
    public Sprite[] starSprites;
    public GameObject winnerPanel;
    public Button exitBt;
    public Text winner;

    Vector3 spawnPoint;
    Color FinnColor = new Color(66/255f, 218/255f, 1.0f);
    Color JackColor = new Color(1.0f, 1.0f, 0.0f);

    Player Jack;
    Player Finn;


    void Start () {
        spawnPoint = GameObject.Find("startPoint").transform.position;
        reset.onClick.AddListener(Replay); 
        exitBt.onClick.AddListener(exit);
        Jack.winCount = 0;
        Finn.winCount = 0;
    }

    void exit()
    {
        Debug.Log("Quit Game!!");
        Application.Quit();
    }

    public void CompareDistance(float JackD, float FinnD)
    {

        string runWinner = JackD < FinnD ? "Jack" : "Finn";

        if (runWinner == "Jack")
            Jack.winCount++;
        else
            Finn.winCount++;
            

        if (Jack.winCount > 0 && Jack.winCount < 3)
            jackStar[Jack.winCount - 1].GetComponent<Image>().sprite = starSprites[1];
        if (Finn.winCount > 0 && Finn.winCount < 3)
            finnStar[Finn.winCount - 1].GetComponent<Image>().sprite = starSprites[1];

        //Final winner
        if(Jack.winCount==3||Finn.winCount==3){
            string Winner = Jack.winCount > Finn.winCount ? "Jack" : "Finn";
            winnerPanel.SetActive(true);
            winner.text = Winner;
            Debug.Log(winner.text);
            winner.color = Winner == "Jack" ? JackColor : FinnColor;
            winner.transform.SetAsLastSibling();
        }
    }

    void Replay(){

        //reset ball's position
        ball.transform.position = spawnPoint;

        //change ball's color
        Color ballColor = ball.gameObject.GetComponentInChildren<Renderer>().material.color;
        ball.gameObject.GetComponentInChildren<Renderer>().material.color = ballColor != FinnColor ? FinnColor : JackColor;

        //move arrow
        Vector3 arrowPosition = arrow.transform.position;
        float diff = -48.0f;
        if (ballColor != FinnColor) diff *= -1;
        arrow.transform.position = new Vector3(arrowPosition.x, arrowPosition.y - diff, arrowPosition.z);

        //clear text
        GameObject replayBtn = GameObject.Find("Replay");
        replayBtn.SetActive(false);
        resultTitle.enabled = false;
        showGauge.enabled = false;
        inputForce.text = "";
    }


}
