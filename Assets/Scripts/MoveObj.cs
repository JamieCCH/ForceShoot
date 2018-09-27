using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveObj : MonoBehaviour {

    private float desiredTimeToDestination = 5.0f;

    public Transform destination;
    public InputField inputForce;
    public Text resultTitle;
    public Text showGauge;
   
    private float pushForce = 0.0f;
    private float timeElapsed = 0.0f;
    private bool isRunning = false;
    Rigidbody rb = null;
    private float acceleration = 0.0f;
    private float expectedForce = 0.0f;

    GameObject replayBtn;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        acceleration = CalculateAcceleration();

        expectedForce = rb.mass * acceleration;
        Debug.Log("expectedForce: " + expectedForce);

        inputForce.text = "";
        resultTitle.enabled = false;

        replayBtn = GameObject.Find("Replay");
        replayBtn.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            pushForce = float.Parse(inputForce.text);
            isRunning = !isRunning;
            //isRunning = true;
        }
        if (!isRunning)
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected float CalculateAcceleration()
    {
        if (desiredTimeToDestination <= 0.0f)
        {
            return 0.0f;
        }

        //this is all based on travelling on the z axis(forward vector)

        //Vi = rb.velocity.z;
        //Vf = ?
        //time = desiredTimeToDestination
        //d = Mathf.abs(transform.postion.z - destination.position.z)
        //a = ?

        //(the formula: ) d = vi*t +0.5*a*t^2
        //so: a = (d - vi*t)/0.5*t^2

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

            //rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

            if (timeElapsed > desiredTimeToDestination)
            {
                isRunning = false;
                Debug.Log("Should stop now");
                rb.velocity = Vector3.zero;
                CalculateDistance();
            }
        }
    }

    void CalculateDistance(){

        float frontEdge = destination.position.z - (destination.transform.localScale.z / 2);
        float backEdge = destination.position.z + (destination.transform.localScale.z / 2);
        float objFrontEdge = transform.position.z + (transform.localScale.z / 2);
        float objBackEdge = transform.position.z - (transform.localScale.z / 2);

        Debug.Log("frontEdge: "+frontEdge);
        Debug.Log("backEdge: " + backEdge);
        Debug.Log("object: " + transform.position.z);

        replayBtn.SetActive(true);
        resultTitle.enabled = true;

        if (objFrontEdge > frontEdge && objBackEdge < backEdge)
        {
            Debug.Log("Bingo!!");
            showGauge.text = "ON";
        }
        if(objBackEdge > backEdge)
        {
            float dist = objBackEdge - backEdge;
            Debug.Log("distance: "+dist);
            Debug.Log("Over shoot.");

            showGauge.text = "over";

        }
        if(objFrontEdge < frontEdge){
            float dist = frontEdge - objFrontEdge;
            Debug.Log("distance: "+dist);
            Debug.Log("Under shoot.");
           
            showGauge.text = "under";
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Time Elapsed: " + timeElapsed);
        //Debug.Log("Velocity: " + rb.velocity);

        //col.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
    }
}
