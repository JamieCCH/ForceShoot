using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveObj : MonoBehaviour {

    float desiredTimeToDestination = 2.5f;

    public Transform destination;
    public InputField inputForce;
    public Text resultTitle;
    public Text showGauge;

    float pushForce = 0.0f;
    float timeElapsed = 0.0f;
    bool isRunning = false;
    Rigidbody rb = null;
    float acceleration = 0.0f;
    float expectedForce = 0.0f;
    GameObject replayBtn = null;
    Vector3 spawnPoint;

    Player Jack;
    Player Finn;

    int playTimes = 0;

    void Start () {
        rb = GetComponent<Rigidbody>();

        acceleration = CalculateAcceleration();

        expectedForce = rb.mass * acceleration;
        Debug.Log("expectedForce: " + expectedForce);

        inputForce.text = "";
        resultTitle.enabled = false;

        replayBtn = GameObject.Find("Replay");
        replayBtn.SetActive(false);

        spawnPoint = GameObject.Find("startPoint").transform.position;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            if (transform.position == spawnPoint)
            {
                playTimes++;
                pushForce = float.Parse(inputForce.text);
                isRunning = !isRunning;
                timeElapsed = 0.0f;
            }
        }
        if (!isRunning)
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected float CalculateAcceleration()
    {
        if (desiredTimeToDestination <= 0.0f)
            return 0.0f;

        float Vi = rb.velocity.z;
        float d = Mathf.Abs(destination.position.z - transform.position.z);
        float a = (d - (Vi * desiredTimeToDestination)) / (0.5f * Mathf.Pow(desiredTimeToDestination, 2.0f));

        return a;
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            timeElapsed += Time.fixedDeltaTime;

            rb.AddForce(transform.forward * pushForce, ForceMode.Force);

            if (timeElapsed > desiredTimeToDestination)
            {
                //Should stop now
                isRunning = false;
                CalculateDistance();
            }
        }
    }

    void CalculateDistance()
    {
        float frontEdge = destination.position.z - (destination.transform.localScale.z / 2);
        float backEdge = destination.position.z + (destination.transform.localScale.z / 2);
        float objFrontEdge = transform.position.z + (transform.localScale.z / 2);
        float objBackEdge = transform.position.z - (transform.localScale.z / 2);
        float dist = Mathf.Abs(objBackEdge - backEdge);

        if (playTimes == 1)
            Jack.resultDistance = dist;
        else
            Finn.resultDistance = dist;

        if (playTimes == 2){

            Debug.Log("Jack got distance = " + Jack.resultDistance);
            Debug.Log("Fin got distance = " + Finn.resultDistance);

            GameObject.Find("HUDPanel").GetComponent<UIScript>().CompareDistance(Jack.resultDistance, Finn.resultDistance);
            Jack.resultDistance = 0.0f;
            Finn.resultDistance = 0.0f;
            playTimes = 0;
        }

        replayBtn.SetActive(true);
        resultTitle.enabled = true;
        showGauge.enabled = true;

        if (objFrontEdge > frontEdge && objBackEdge < backEdge)
            showGauge.text = "ON";

        if(objBackEdge > backEdge)
            showGauge.text = "over";

        if(objFrontEdge < frontEdge)
            showGauge.text = "under";

    }


}
