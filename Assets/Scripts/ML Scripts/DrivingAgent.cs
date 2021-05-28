using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class DrivingAgent : Agent
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Quaternion targetRotation;
    private Transform targetTransform;
    
    
    [SerializeField] private carControllerVer4 controller;
    
    public bool beginningEpisode;
    public float errorCount;
    public Transform firstCheckpointTransform;

    private void Awake()
    {
        errorCount = 0;
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        targetTransform = firstCheckpointTransform;
        controller.StopCompletely();
        errorCount = 0;
        beginningEpisode = true;
    }
    
    

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Dot(transform.forward, targetTransform.forward));
        AddReward(-0.001f); //Para que el coche intente ir un poco más rápido
    }

    public override void OnActionReceived(ActionBuffers actions) // Interpretación de las decisiones tomadas
    {
        float accel = actions.ContinuousActions[0];
        float steer = actions.ContinuousActions[1];
        bool isDrifting = actions.DiscreteActions[0] == 1;

        SendInput(steer, accel, false, isDrifting);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut) // "Override" de las decisiones del modelo. Para testing y grabación de demos
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxis("Vertical");
        continuousActions[1] = Input.GetAxis("Horizontal");
        discreteActions[0] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 3) // Layer 3 = Capa "Wall"
        {
            AddReward(-1f); // Al colisionar se castiga al modelo con -1
             
             //Código para automatizar el ciclo de episodios
             
            /*errorCount++;
            if (errorCount >= 5)
            {
                EndEpisode();
            }*/
            
        }
    }

    

    public void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 3)
        {
            AddReward(-0.1f); 
            // Por cada frame que el modelo siga colisionando con un muro,
            // se añade un pequeño castigo más

           /* errorCount += 0.01f;
            if(errorCount >= 5) EndEpisode(); */
        }
    }

    public void SetNextCheckpointTransform(Transform transform)
    {
        targetTransform = transform;
    } 
    
    private void SendInput(float steer, float accel, bool isBraking, bool isDrifting)
    {
        controller.GetInputFromAI(steer, accel, isBraking, isDrifting, false, false);
        controller.HandleMotor();
        controller.HandleSteering();
        controller.UpdateWheels();
        controller.AddDownForce();
    }
}
