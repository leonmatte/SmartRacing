using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.Vehicles.Car;

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
    private int roundedSpeed;
    private float speed;
    private Rigidbody rigidbodyCar;
    public int lapCounter;

    [SerializeField] public float maxSpeedIA = 250;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float m_Downforce = 100f;
    [SerializeField] private float topSpeed;
    private float topSpeedAux;

    public bool driving;
    public bool isPlayer;
    private Transform lastCheckpointTransform;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[2];

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private static int NoOfGears = 5;
    private int m_GearNum;
    private float m_GearFactor;
    [SerializeField] private float m_RevRangeBoundary = 1f;
    [SerializeField] private float m_SlipLimit;
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    public GameObject direccionContraria;

    private void Start()
    {
        lastCheckpointTransform = transform;
        rigidbodyCar = GetComponent<Rigidbody>();
        topSpeedAux = topSpeed;
        lapCounter = 0;
        driving = true;
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
            HandleReset();
            HandleWrongWay();
            CalculateRevs();
            GearChanging();
            CheckForWheelSpin();
        }
    }

    private void GetInput()
    {
        if(driving){ 
            horizontalInput = Input.GetAxis(HORIZONTAL);
            verticalInput = Input.GetAxis(VERTICAL);
            isBreaking = Input.GetKey(KeyCode.Space);
            isDrifting = Input.GetKey(KeyCode.LeftShift);
        }
    }

    public void GetInputFromAI(float horizontalInput, float verticalInput, bool isBreaking, bool isDrifting, bool inputW, bool inputS)
    {
        if (driving)
        {
            this.horizontalInput = horizontalInput;
            this.verticalInput = verticalInput;
            this.isBreaking = isBreaking;
            this.isDrifting = isDrifting;
        }
    }

    private void ShowSpeed()
    {
        speed = rigidbodyCar.velocity.magnitude * 3.6f;
        roundedSpeed = Mathf.RoundToInt(speed);
        speedText.SetText("Velocidad: " + roundedSpeed + "Km/h");
    }

    private void HandleWrongWay()
    {
        float targetAngle = transform.eulerAngles.y - lastCheckpointTransform.eulerAngles.y;
        if (Mathf.Abs(targetAngle) > 135 && Mathf.Abs(targetAngle) < 225)
        {
            //print("WRONG WAY " + targetAngle);
            direccionContraria.SetActive(true);
        }
        else 
        {
            direccionContraria.SetActive(false);
        }
    }

    public void HandleReset()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rigidbodyCar.velocity = new Vector3(0, 0, 0);
            transform.position = lastCheckpointTransform.position - 3 * Vector3.up;
            transform.rotation = lastCheckpointTransform.rotation;
            print("OW");
        }
    }

    public void HandleMotor()
    {

        RearSpeed();

        if(frontLeftWheelCollider.rpm <= 1500f && roundedSpeed < topSpeed)
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
        if (roundedSpeed < 60)
        {
            maxSteerAngle = 30;
        }
        else if (roundedSpeed > 60 && roundedSpeed < 120)
        {
            maxSteerAngle = 15;
        }
        else if (roundedSpeed > 120)
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
        return roundedSpeed;
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
        var targetGearFactor = Mathf.InverseLerp(f*m_GearNum, f*(m_GearNum + 1), Mathf.Abs(roundedSpeed/topSpeedAux));
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

    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        WheelCollider[] m_WheelColliders = {rearLeftWheelCollider, rearRightWheelCollider};
        
        // loop through all wheels
        for (int i = 0; i < 2; i++)
        {
            WheelHit wheelHit;
            m_WheelColliders[i].GetGroundHit(out wheelHit);

            // is the tire slipping above the given threshhold
            if (isDrifting)
            {
                // avoiding all four tires screeching at the same time
                // if they do it can lead to some strange audio artefacts
                if (!AnySkidSoundPlaying())
                {
                    m_WheelEffects[i].PlayAudio();
                    
                }
                m_WheelEffects[i].EmitTyreSmoke();
                continue;
            }

            // if it wasnt slipping stop all the audio
            if (m_WheelEffects[i].PlayingAudio)
            {
                m_WheelEffects[i].StopAudio();
            }
            // end the trail generation
            m_WheelEffects[i].EndSkidTrail();
        }
    }

    private bool AnySkidSoundPlaying()
    {
        for (int i = 0; i < 2; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
    
    public void SetLastCheckpointTransform(Transform transform)
    {
        lastCheckpointTransform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("Crash");
    }
}
