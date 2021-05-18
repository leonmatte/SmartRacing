using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public GameObject anyname;

    public Rigidbody rigidbody;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f;
    public float dragOnGround = 3f;

    private float speedInput, turnInput;

    private bool grounded;

    public bool isPlayer = true;

    public LayerMask whatIsGround;
    public float groundRayLength;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer) GetInputs();

    }

    public void GetInputs()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000;
        }

        turnInput = Input.GetAxis("Horizontal");

        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);
        transform.position = rigidbody.transform.position;
    }

    public void GetInputs(float vertical, float horizontal)
    {
        speedInput = 0f;
        if (vertical > 0)
        {
            speedInput = vertical * forwardAccel * 1000;
        }
        else if (vertical < 0)
        {
            speedInput = vertical * reverseAccel * 1000;
        }

        turnInput = horizontal;

        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * horizontal, 0f));
        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);
        transform.position = rigidbody.transform.position;
    }

    void FixedUpdate()
    {

        grounded = false;
        RaycastHit hit;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        if (grounded)
        {
            rigidbody.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                rigidbody.AddForce(transform.forward * speedInput);
            }
        } else
        {
            rigidbody.drag = 0.1f;
            rigidbody.AddForce(Vector3.up * -gravityForce * 100);
        }

        //rigidbody.AddForce(transform.forward * forwardAccel * 1000);
        
    }

}
