using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down")]
    [SerializeField] private float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    [SerializeField] private float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing")]
    [SerializeField] private float responsiveness = 10f;
    [SerializeField] private float moveSpeed = 1.5f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float responseModifier => (rb.mass / 10f) * responsiveness;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Pitch");
        pitch = Input.GetAxis("Roll");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
            throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl))
            throttle -= throttleIncrement;
        throttle = Mathf.Clamp01(throttle);
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        ApplyThrust();
        ApplyTorque();
    }

    private void ApplyThrust()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
    }

    private void ApplyTorque()
    {
        Vector3 torque = new Vector3(pitch, yaw, roll) * responseModifier;
        rb.AddRelativeTorque(torque);
    }
}
