using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HeaderAttribute("Bid Stats")]
    public float throttleIncremnet = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    public Animator animator;

    public float lift = 50f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private bool isStarted;



    Rigidbody rb;
    private bool isGravityEnabled = true;


    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();    
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle += throttleIncremnet;
            if(!isStarted)
            {
                animator.SetBool("isStarting", true);
                isStarted = true;
            }
            animator.SetBool("isFlying", true);
            animator.SetBool("isGleiding", false);
        }

        if (Input.GetKey(KeyCode.C))
        {
            throttle -= throttleIncremnet;
            animator.SetBool("isFlying", false);
            animator.SetBool("isGleiding", true);

        }
        if (Input.GetKeyDown(KeyCode.G)) ToggleGravity();


        throttle = Mathf.Clamp(throttle, 0f , 100f);
    }

    private void Update()
    {
       HandleInputs();
    }

    private void FixedUpdate()
    {
        rb.AddForce(-transform.right * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * roll * responseModifier);
        rb.AddTorque(-transform.forward *  pitch * responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private void ToggleGravity()
    {
        isGravityEnabled = !isGravityEnabled;
        rb.useGravity = isGravityEnabled;
    }

}
