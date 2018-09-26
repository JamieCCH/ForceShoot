using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float acceleration = 1.0f;

    private float desiredTimeToDestination = 5.0f;
    public Transform destination;
    private float timeElapsed = 0.0f;
    private bool isRuuning = false;
    Rigidbody rb = null;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRuuning = !isRuuning;

            if(isRuuning){
                acceleration = CalculateAcceleration();
            }
        }
    }

    protected float CalculateAcceleration(){

        //this is all based off travelling on the z axis(forward vector)

        //Vi = rb.velocity.z;
        //Vf = ?
        //time = desiredTimeToDestination
        //d = Mathf.abs(transform.postion.z - destination.position.z)
        //a = ?

        //d = vi*t +0.5*a*t^2
        //a = (d - vi*t)/0.5*t^2

        float Vi = rb.velocity.z;
        float d = Mathf.Abs(transform.position.z - destination.position.z);
        float a = (d - Vi * desiredTimeToDestination) / (0.5f * Mathf.Pow(desiredTimeToDestination, 2.0f));
       
        return a;
    }


    void FixedUpdate()
    {
        if(isRuuning){
            timeElapsed += Time.fixedDeltaTime;
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRuuning)
        {
            other.gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            Debug.Log("Time elapsed: " + timeElapsed);
            Debug.Log("Velocity: " + rb.velocity);

            isRuuning = false;
        }
    }
}
