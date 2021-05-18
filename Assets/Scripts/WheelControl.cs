using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{

    public WheelCollider WC;
    public float torque = 200;
    public GameObject wheel;

    [SerializeField]public float maxSteerAngle;

    // Start is called before the first frame update
    void Start()
    {
        WC = this.GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float a = Input.GetAxis("Vertical");
        float b = Input.GetAxis("Horizontal");

        Go(a, b);

    }

    void Go(float accel, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        float thrustTorque = accel * torque;
        WC.motorTorque = thrustTorque;
        WC.steerAngle = steer;

        Quaternion quat;
        Vector3 position;
        WC.GetWorldPose(out position, out quat);
        wheel.transform.position = position;
        wheel.transform.rotation = quat;
    }
 }
