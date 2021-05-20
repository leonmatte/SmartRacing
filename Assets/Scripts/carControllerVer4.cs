using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class carControllerVer4 : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    private bool isDrifting;
    private int speed;
    private Rigidbody rigidbodyCar;

    [SerializeField] public float maxSpeedIA = 250;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float m_Downforce = 100f;
    [SerializeField] private int topSpeed;
    public bool isPlayer;
    private Vector3 nextCheckpointPosition;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Start()
    {
        rigidbodyCar = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        if(isPlayer){
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            AddDownForce();
            ShowSpeed();
        }
        
    }
    
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
        isDrifting = Input.GetKey(KeyCode.LeftShift);
    }

    public void GetInputFromAI(float horizontalInput, float verticalInput, bool isBreaking, bool isDrifting, bool inputW, bool inputS)
    {
        this.horizontalInput = horizontalInput;
        this.verticalInput = verticalInput;
        this.isBreaking = isBreaking;
        this.isDrifting = isDrifting;
    }

    private void ShowSpeed()
    {
        speed = Mathf.RoundToInt(rigidbodyCar.velocity.magnitude * 3.6f);
        speedText.SetText("Velocidad: " + speed + "Km/h");
    }

    public void HandleMotor()
    {
        if(frontLeftWheelCollider.rpm <= 1500f && speed < topSpeed)
            {
                frontRightWheelCollider.motorTorque = verticalInput * motorForce * Time.fixedDeltaTime;
                frontLeftWheelCollider.motorTorque = verticalInput * motorForce * Time.fixedDeltaTime;
                rearLeftWheelCollider.motorTorque = verticalInput * motorForce * Time.fixedDeltaTime;
                rearRightWheelCollider.motorTorque = verticalInput * motorForce * Time.fixedDeltaTime;
            }
            else
            {
                frontRightWheelCollider.motorTorque = verticalInput * (motorForce / 10) * Time.fixedDeltaTime;
                frontLeftWheelCollider.motorTorque = verticalInput * (motorForce / 10) * Time.fixedDeltaTime;
                rearLeftWheelCollider.motorTorque = verticalInput * (motorForce / 10) * Time.fixedDeltaTime;
                rearRightWheelCollider.motorTorque = verticalInput * (motorForce / 10) * Time.fixedDeltaTime;
            }
            
        //Si el boton de frenado esta apretado,  el brakeForce es = brakeForce, sino, es 0;
        currentbreakForce = isBreaking ? breakForce : 0f * Time.fixedDeltaTime;
        ApplyBreaking();
        ApplyDrifting();
        
    }

    private void ApplyDrifting()
    {
        
        //Si el boton de drifting esta pulsado, cambia la adherencia de las ruedas traseras.
        if(isDrifting)
        {
            
            //Cambiamos el stiffness de Sideways Friction de las ruedas traseras para hacer que el coche derrape
            WheelFrictionCurve sFriction = rearLeftWheelCollider.sidewaysFriction;
            sFriction.stiffness = 1.2f;
            rearLeftWheelCollider.sidewaysFriction = sFriction;
            rearRightWheelCollider.sidewaysFriction = sFriction;

        }
        else
        {
            //Volvemos a poner el stiffness a su estado original
            WheelFrictionCurve sFriction = rearLeftWheelCollider.sidewaysFriction;
            sFriction.stiffness = 2f;
            rearLeftWheelCollider.sidewaysFriction = sFriction;
            rearRightWheelCollider.sidewaysFriction = sFriction;

        }
        //Freno de ruedas delanteras
        //rearLeftWheelCollider.brakeTorque = currentDriftForce;
        //rearRightWheelCollider.brakeTorque = currentDriftForce;

        //Metodo en caso de querer cambiar la adherencia cuando se pise x terreno.
        //WheelHit hit;
        //if (rearLeftWheelCollider.GetGroundHit(out hit))
        //{ 
        //WheelFrictionCurve fFriction = rearLeftWheelCollider.forwardFriction;
        //fFriction.stiffness = hit.collider.material.staticFriction;
        //rearLeftWheelCollider.forwardFriction = fFriction;
        //}
    }

    public void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    public void HandleSteering()
    {
        if (speed < 60)
        {
            maxSteerAngle = 30;
        }
        else if (speed > 60 && speed < 120)
        {
            maxSteerAngle = 15;
        }
        else if (speed > 120)
        {
            maxSteerAngle = 5;
        }
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    public void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    
    public void AddDownForce()
    {
        frontLeftWheelCollider.attachedRigidbody.AddForce(-transform.up*m_Downforce*
                                                          frontLeftWheelCollider.attachedRigidbody.velocity.magnitude);
    }

    public int GetSpeed()
    {
        return speed;
    }
    
    public void SetNextCheckpointPosition(Vector3 position)
    {
        this.nextCheckpointPosition = position;
        print(nextCheckpointPosition);
    }
    
}