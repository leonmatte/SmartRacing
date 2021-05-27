using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;

public class MLCheckpointTracker : MonoBehaviour
{

    
    [SerializeField] private GameObject carParent;
    
    private List<Transform> carTransformList;
    private List<DrivingAgent> agentList;
    private List<MLCheckpoint> checkpointList;
    private List<int> nextCheckpointList;
    

    void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        agentList = carParent.GetComponentsInChildren<DrivingAgent>().ToList();
        
        print("Agent List: " + agentList.Count);
        
        BoxCollider[] colliders = carParent.GetComponentsInChildren<BoxCollider>();
        carTransformList = new List<Transform>();
        
        foreach (BoxCollider collider in colliders)
        {
            carTransformList.Add(collider.transform);
        }
        
        print("Transform List: " + carTransformList.Count);

        checkpointList = new List<MLCheckpoint>();

        foreach (Transform checkpoint in checkpointsTransform)
        {
            MLCheckpoint checkpointObject = checkpoint.GetComponent<MLCheckpoint>();
            checkpointObject.SetTrackCheckpoints(this);
            checkpointList.Add(checkpointObject);
        }

        nextCheckpointList = new List<int>();
        foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointList.Add(0);
        }
    }

    public void CarThroughCheckpoint(MLCheckpoint checkpoint, Transform carTransform)
    {
        DrivingAgent agent = agentList[carTransformList.IndexOf(carTransform)];

        if (agent.beginningEpisode)
        {
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = 0;
            print("NEW EPISODE " + nextCheckpointList[carTransformList.IndexOf(carTransform)]);
            agent.beginningEpisode = false;
        }
        
        int nextCheckpointIndex = nextCheckpointList[carTransformList.IndexOf(carTransform)];
        
        if(checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            print("Lessgooo " + carTransformList.IndexOf(carTransform));
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;

            agent.AddReward(1f);
            if (nextCheckpointIndex == checkpointList.Count)
            {
                agent.AddReward(5f);
                agent.EndEpisode();
            }
            
            if (nextCheckpointList[carTransformList.IndexOf(carTransform)] == 0)
            {
                agent.AddReward(5f);
            }
            agent.SetNextCheckpointTransform(checkpointList[nextCheckpointList[carTransformList.IndexOf(carTransform)]].transform);
            
        }
        else
        {
            print("whutttt " + carTransformList.IndexOf(carTransform));
            agent.AddReward(-1f);
            agent.errorCount++;
        }
    }

    public void ResetCheckpointIndex(Transform carTransform)
    {
        if (nextCheckpointList != null){
            print("INDEX OF CAR " + carTransformList.IndexOf(carTransform) + " WAS PREVIOUSLY " +
                  nextCheckpointList[carTransformList.IndexOf(carTransform)]);
            
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = 0;
            
            print("INDEX OF CAR " + carTransformList.IndexOf(carTransform) + " CHANGED TO " +
                  nextCheckpointList[carTransformList.IndexOf(carTransform)]);
        } else print("NULL");
        
    } 
}
