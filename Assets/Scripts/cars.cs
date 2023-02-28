using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour
{
    public float speed = 10.0f;
    public float acceleracion = 10.0f;
    public float braking = 10.0f;
    public Vector3 direction;

    public float ratationForce = 10.0f;

    float accelerationInput;

    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        direction += new Vector3(HorizontalInput, 0, VerticalInput) * acceleracion * Time.deltaTime;

        direction = Vector3.ClampMagnitude(direction, speed);

        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            direction = Vector3.Lerp(direction, Vector3.zero, braking * Time.deltaTime);
        }

        transform.position += direction * Time.deltaTime;

        if (accelerationInput > 0)
        {
            transform.Rotate(Vector3.up, HorizontalInput * ratationForce * Time.deltaTime);

        }



    }
}
