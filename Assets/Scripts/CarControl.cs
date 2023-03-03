using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f, dragOnGround;

    private float speedInput, turnInput;


    private float groundRayLenght = .5f;
    public Transform groundRayPoint;

    public Transform lefFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;

    private void Start()
    {
        rb.transform.parent = null;
    }

    private void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 100f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 100f;
        }

        turnInput = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));


        lefFrontWheel.localRotation = Quaternion.Euler(lefFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, lefFrontWheel.localRotation.eulerAngles.z);

        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

        transform.position = rb.transform.position;
    }

    private void FixedUpdate()
    {
        //grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLenght))
        {

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        rb.drag = dragOnGround;
        if (Mathf.Abs(speedInput) > 0)
        {
            rb.AddForce(transform.forward * speedInput);
        }

        else
        {
            rb.drag = 0.1f;
            rb.AddForce(Vector3.up * -gravityForce * 100f);
        }

    }
}
