using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{

    [SerializeField] private List<Transform> carTransformList;

    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointList;

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

        nextCheckpointList = new List<int>();
        foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointList.Add(0);
        }
    }

    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform carTransform)
    {
        int nextCheckpointIndex = nextCheckpointList[carTransformList.IndexOf(carTransform)];
        if(checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            print("Lessgooo " + carTransformList.IndexOf(carTransform));
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;
        }
        else
        {
            print("whutttt " + carTransformList.IndexOf(carTransform));
        }
    }
}
