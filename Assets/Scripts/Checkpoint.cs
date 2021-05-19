using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update

    private CheckpointTracker checkpointTracker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BoxCollider>(out BoxCollider player))
        {
            checkpointTracker.PlayerThroughCheckpoint(this);
        }
    }

    public void SetTrackCheckpoints(CheckpointTracker tracker)
    {
        this.checkpointTracker = tracker;
    }
    
}
