using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyMovement : MonoBehaviour
{
    public float speed = 0.6f; // movement speed
    public float rotationSpeed = 50f; // rotation speed
    public int babyAge = 0; // baby age in months
    private float[] babyWeightByMonth = new float[24] {3.5f, 4.0f, 5.0f, 5.5f, 6.0f, 6.5f, 7.0f, 7.5f, 8.0f, 8.5f, 9.0f, 9.5f, 10.0f, 10.5f, 11.0f, 11.5f, 12.0f, 12.5f, 13.0f, 13.5f, 14.0f, 14.5f, 15.0f, 15.5f}; // weights for each month of age
    private float[] babyHeightByMonth = new float[24] {0.49f, 0.535f, 0.579f, 0.614f, 0.642f, 0.662f, 0.678f, 0.692f, 0.705f, 0.717f, 0.728f, 0.739f, 0.75f, 0.76f, 0.769f, 0.778f, 0.787f, 0.796f, 0.805f, 0.814f, 0.823f, 0.831f, 0.839f, 0.847f}; // heights for each month of age
    private Rigidbody rb; // Rigidbody component
    private float nextChangeTime = 0f; // time to change state
    private Vector3 targetVelocity; // target velocity
    private float rotationAngle; // angle to rotate in the next frame

    // possible states
    private enum State
    {
        Stopping,
        MovingStraight,
        Rotating,
    }

    private State state = State.MovingStraight; // initial state

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // get Rigidbody component
        ChangeState(); // change to a random state
        SetBabyWeight(); // set baby weight based on age
        SetBabyHeight(); // set baby height based on age
    }

    void Update()
    {
        if (Time.time >= nextChangeTime) // if it is time to change the state
        {
            ChangeState(); // change the state
            nextChangeTime = Time.time + 1f; // set the next change time
        }

        switch (state)
        {
            case State.MovingStraight:
                // move only in the positive X direction of the local coordinate system
                targetVelocity = transform.right * speed;
                rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime);
                break;

            case State.Rotating:
                // rotate around the up axis
                transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);
                break;

            case State.Stopping:
                // stop the movement
                targetVelocity = Vector3.zero;
                rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime);
                break;
        }
    }

    void ChangeState()
    {
        // choose a new random state
        state = (State)Random.Range(0, 3);

        if (state == State.Rotating)
        {
            // if the new state is rotating, choose a new random rotation angle
            rotationAngle = Random.Range(-rotationSpeed, rotationSpeed);
        }
    }

    void SetBabyWeight()
    {
        // set baby weight based on age 
        if (babyAge >= 0 && babyAge < 24) {
            rb.mass = babyWeightByMonth[babyAge];
        } else {
            Debug.LogError("Invalid baby age: " + babyAge);
        }
    }

    void SetBabyHeight()
    {
        // set baby height based on age 
        if (babyAge >= 0 && babyAge < 24) {
            transform.localScale = new Vector3(babyHeightByMonth[babyAge], babyHeightByMonth[babyAge]/3, babyHeightByMonth[babyAge]/2);
        } else {
            Debug.LogError("Invalid baby age: " + babyAge);
        }
    }
}