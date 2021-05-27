using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update

    private MLCheckpointTracker checkpointTracker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BoxCollider>(out BoxCollider player))
        {
            checkpointTracker.CarThroughCheckpoint(this, other.transform);
        }
    }

    public void SetTrackCheckpoints(MLCheckpointTracker tracker)
    {
        this.checkpointTracker = tracker;
    }
    
}
