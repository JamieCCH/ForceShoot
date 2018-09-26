using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour {

    public float timeToImpact = 5.0f;
    public Transform targetTransform;
    public float targetSpeed = 1.0f;

    private Rigidbody m_rb;
    public float pushForce = 0.0f;
    private float m_elapsedTime = 0.0f;
    private bool m_isRunning = false;

    private float m_acceleration = 0.0f;


    // Use this for initialization
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_isRunning = !m_isRunning;
            //m_acceleration = CalculateAcceleration();
            if (!m_isRunning)
            {
                Debug.Log("Time Elapsed: " + m_elapsedTime);
                Debug.Log("Velocity: " + m_rb.velocity);
                m_elapsedTime = 0.0f;
            }
        }
    }

    float CalculateAcceleration()
    {
        if (timeToImpact <= 0.0f)
        {
            return 0.0f;
        }

        float displacement = targetTransform.position.z - transform.position.z;
        displacement += (targetSpeed * timeToImpact);

        float acceleration = (displacement - (m_rb.velocity.z * timeToImpact)) / (0.5f * Mathf.Pow(timeToImpact, 2.0f));
        return acceleration;
    }

    void FixedUpdate()
    {
        if (m_isRunning)
        {
            m_elapsedTime += Time.fixedDeltaTime;
            m_rb.AddForce(transform.forward * pushForce, ForceMode.Force);
            //m_rb.AddForce(transform.forward * m_acceleration, ForceMode.Acceleration);

            if(m_elapsedTime > timeToImpact)
            {
                m_isRunning = false;
                Debug.Log("Should stop");
                m_rb.velocity = Vector3.zero;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {

        col.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;

        Debug.Log("Time Elapsed: " + m_elapsedTime);
        Debug.Log("Velocity: " + m_rb.velocity);

        //Debug.Log("a= " + m_acceleration);
        //Debug.Log("Force: " + m_rb.mass * m_acceleration);
    }
}
