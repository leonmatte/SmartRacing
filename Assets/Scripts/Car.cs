﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public bool isPlayer = true;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    private bool isDrifting;
    private bool inputW, inputS;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float velAcceleration;
    private int direction = 1;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Update()
    {
        
        if(isPlayer) GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
        isDrifting = Input.GetKey(KeyCode.LeftShift);
        inputW = Input.GetKey(KeyCode.W);
        inputS = Input.GetKey(KeyCode.S);
    }

    public void GetInputWithParam(float horizontalInput, float verticalInput, bool isBreaking, bool isDrifting, bool inputW, bool inputS)
    {
        this.horizontalInput = horizontalInput;
        this.verticalInput = verticalInput;
        this.isBreaking = isBreaking;
        this.isDrifting = isDrifting;
        this.inputW = inputW;
        this.inputS = inputS;
    }

    private void HandleMotor()
    {
        if (inputW)
        {
            if(frontLeftWheelCollider.rpm <= 1000)
            {
                frontRightWheelCollider.motorTorque = velAcceleration * direction * motorForce * Time.deltaTime;
                frontLeftWheelCollider.motorTorque = velAcceleration * direction * motorForce * Time.deltaTime;
                rearLeftWheelCollider.motorTorque = velAcceleration * direction * motorForce * Time.deltaTime;
                rearRightWheelCollider.motorTorque = velAcceleration * direction * motorForce * Time.deltaTime;
            }
            else
            {
                frontRightWheelCollider.motorTorque = direction * motorForce * Time.deltaTime;
                frontLeftWheelCollider.motorTorque = direction * motorForce * Time.deltaTime;
                rearLeftWheelCollider.motorTorque = direction * motorForce * Time.deltaTime;
                rearRightWheelCollider.motorTorque = direction * motorForce * Time.deltaTime;
            }
            
        }
        else if (inputS)
        {
           
            frontRightWheelCollider.motorTorque = -direction * motorForce * Time.deltaTime;
            frontLeftWheelCollider.motorTorque = -direction * motorForce * Time.deltaTime;
            rearLeftWheelCollider.motorTorque = -direction * motorForce * Time.deltaTime;
            rearRightWheelCollider.motorTorque = -direction * motorForce * Time.deltaTime;
        }
        else
        {
            frontRightWheelCollider.motorTorque = 0;
            frontLeftWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
        }
        //Si el boton de frenado esta apretado,  el brakeForce es = brakeForce, sino, es 0;
        currentbreakForce = isBreaking ? breakForce : 0f * Time.deltaTime;
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
            sFriction.stiffness = 0.4f;
            rearLeftWheelCollider.sidewaysFriction = sFriction;
            rearRightWheelCollider.sidewaysFriction = sFriction;

        }
        else
        {
            //Volvemos a poner el stiffness a su estado original
            WheelFrictionCurve sFriction = rearLeftWheelCollider.sidewaysFriction;
            sFriction.stiffness = 1.4f;
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

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
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
}
