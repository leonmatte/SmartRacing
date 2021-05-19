using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{

    private List<Checkpoint> checkpointList;
    private int nextCheckpoint;

    void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();

        foreach (Transform checkpoint in checkpointsTransform)
        {
            Checkpoint checkpointObject = checkpoint.GetComponent<Checkpoint>();
            checkpointObject.SetTrackCheckpoints(this);
            checkpointList.Add(checkpointObject);
        }

        nextCheckpoint = 0;
    }

    public void PlayerThroughCheckpoint(Checkpoint checkpoint)
    {
        if(checkpointList.IndexOf(checkpoint) == nextCheckpoint)
        {
            print("Lessgooo");
            nextCheckpoint = (nextCheckpoint + 1) % checkpointList.Count;
        }
        else
        {
            print("whutttt");
        }
    }
}
