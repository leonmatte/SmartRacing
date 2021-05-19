using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AltCarAIController : MonoBehaviour
{
    
    [SerializeField] [Range(0, 1)] private float m_CautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
    [SerializeField] [Range(0, 180)] private float m_CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
    [SerializeField] private float m_CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
    [SerializeField] private float m_SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
    [SerializeField] private float m_AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
    [SerializeField] private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
    [SerializeField] private float m_LateralWanderDistance = 3f;                              // how far the car will wander laterally towards its target
    [SerializeField] private float m_LateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
    [SerializeField] [Range(0, 1)] private float m_AccelWanderAmount = 0.1f;                  // how much the cars acceleration will wander
    [SerializeField] private float m_AccelWanderSpeed = 0.1f;                                 // how fast the cars acceleration wandering will fluctuate
    [SerializeField] private bool m_Driving;                                                  // whether the AI is currently actively driving or stopped.
    [SerializeField] private Transform m_Target;                                              // 'target' the target object to aim for.
    [SerializeField] private bool m_StopWhenTargetReached;                                    // should we stop driving when we reach the target?
    [SerializeField] private float m_ReachTargetThreshold = 2;
        
    private float m_RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    private carControllerVer4 m_CarController;    // Reference to actual car controller we are controlling
    private Rigidbody m_Rigidbody;
    private bool wantsToDrift;
    private RaycastHit hit;
    private bool reverse;
    public Transform raycastPoint;
    public float raycastLength;
    public LayerMask mask;
    
    // Start is called before the first frame update
    void Awake()
    {
        m_CarController = GetComponent<carControllerVer4>();

        m_RandomPerlin = Random.value * 100;
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (!m_Driving)
        {
            m_CarController.GetInputFromAI(0, 0, true, false, false, false);
        }

        else
        {
            Vector3 fwd = transform.forward;
            if (m_Rigidbody.velocity.magnitude > m_CarController.maxSpeed * 0.1f)
            {
                fwd = m_Rigidbody.velocity;
            }

            float desiredSpeed = m_CarController.maxSpeed;
            
            BrakeCondition(fwd, out desiredSpeed);

            Vector3 offsetTargetPos = m_Target.position;

            offsetTargetPos += m_Target.right * (Mathf.PerlinNoise(Time.time*m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) * m_LateralWanderDistance;

            float accelBrakeSensitivity = (desiredSpeed < m_CarController.speed)
                ? m_BrakeSensitivity
                : m_AccelSensitivity;

            float accel = 0;

            if (Physics.Raycast(raycastPoint.position, transform.forward, out hit, mask))
            {
                reverse = true;
            }
            else
            {
                accel = Mathf.Clamp((desiredSpeed - m_CarController.speed) * accelBrakeSensitivity, -1, 1);
                reverse = false;
            }
            
            accel *= (1 - m_AccelWanderAmount) +
                     (Mathf.PerlinNoise(Time.time*m_AccelWanderSpeed, m_RandomPerlin)*m_AccelWanderAmount);
            
            Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);
            
            
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;

            Debug.Log(targetAngle);

            if (targetAngle > Mathf.Abs(15)) wantsToDrift = true;
            else wantsToDrift = false;

            float steer = Mathf.Clamp(targetAngle*m_SteerSensitivity, -1, 1)*Mathf.Sign(m_CarController.speed);
            if (reverse) steer *= -1;

            SendInput(steer, accel);
            
            if (m_StopWhenTargetReached && localTarget.magnitude < m_ReachTargetThreshold)
            {
                m_Driving = false;
            }
        }
    }

    private void SendInput(float steer, float accel)
    {
        m_CarController.GetInputFromAI(steer, accel, false, wantsToDrift, accel > 0, reverse);
        m_CarController.HandleMotor();
        m_CarController.HandleSteering();
        m_CarController.UpdateWheels();
        m_CarController.AddDownForce();
    }
    
    public void SetTarget(Transform target)
    {
        m_Target = target;
        m_Driving = true;
    }

    private void BrakeCondition(Vector3 fwd, out float desiredSpeed)
    {
        float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);
        float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
        float cautiosnessRequired =
            Mathf.InverseLerp(0, m_CautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
        desiredSpeed = Mathf.Lerp(m_CarController.maxSpeed, m_CarController.maxSpeed * m_CautiousSpeedFactor,
            cautiosnessRequired);
    }
}
