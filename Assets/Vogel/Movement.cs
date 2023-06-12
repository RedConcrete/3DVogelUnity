using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HeaderAttribute("Bid Stats")]
    public float throttleIncremnet = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;

    public float lift = 50f;


    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f);
        }
    }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncremnet;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncremnet;
        throttle = Mathf.Clamp(throttle, 0f , 100f);
    }

    private void Update()
    {
       HandleInputs();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * roll * responseModifier);
        rb.AddTorque(-transform.forward * pitch * responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

}
