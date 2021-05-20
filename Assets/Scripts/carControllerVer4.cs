﻿using System;
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
    public float rpm;

    [SerializeField] public float maxSpeedIA = 250;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float m_Downforce = 100f;
    [SerializeField] private int topSpeed;
    private int topSpeedAux;

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
    
    [SerializeField] private static int NoOfGears = 5;
    private int m_GearNum;
    private float m_GearFactor;
    [SerializeField] private float m_RevRangeBoundary = 1f;
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    private void Start()
    {
        rigidbodyCar = GetComponent<Rigidbody>();
        topSpeedAux = topSpeed;

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
            CalculateRevs();
            GearChanging();
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

        
        RearSpeed();
        
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
        AccelInput = verticalInput = Mathf.Clamp(verticalInput, 0, 1);


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

    public void RearSpeed()
    {
        if (verticalInput == -1)
        {
            topSpeed = 40;
        }
        else
        {
            topSpeed = topSpeedAux;
        }
    }
    
    private void GearChanging()
    {
        float f = Mathf.Abs(speed/topSpeedAux);
        float upgearlimit = (1/(float) NoOfGears)*(m_GearNum + 1);
        float downgearlimit = (1/(float) NoOfGears)*m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }
    
    private void CalculateGearFactor()
    {
        float f = (1/(float) NoOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f*m_GearNum, f*(m_GearNum + 1), Mathf.Abs(speed/topSpeedAux));
        m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime*5f);
    }
    
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value)*from + value*to;
    }
    
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor)*(1 - factor);
    }
    
    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum/(float) NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
    }
    
}