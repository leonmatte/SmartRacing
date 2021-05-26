using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{

    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private List<carControllerVer4> controllerList;
    private int positions;

    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointList;

    void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();

        positions = 0;

        foreach (carControllerVer4 controller in controllerList)
        {
            if (controller.isPlayer)
            {
                // Implementar LapCounter UI
            }
        }

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
        if(checkpointList.IndexOf(checkpoint) == nextCheckpointIndex) // Si el coche pasa por el checkpoint que le toca
        {
            if (nextCheckpointIndex == 0) // Si el checkpoint es el primero
            {
                controllerList[carTransformList.IndexOf(carTransform)].lapCounter++; // Siguiente vuelta
                print("Coche: " + carTransformList.IndexOf(carTransform) + ", vuelta: " +
                      controllerList[carTransformList.IndexOf(carTransform)].lapCounter);
                if (controllerList[carTransformList.IndexOf(carTransform)].lapCounter > 3) // Si el coche ha completado las tres vueltas
                {
                    controllerList[carTransformList.IndexOf(carTransform)].driving = false; // El coche deja de correr
                    positions++; // Otro coche más ha terminado la carrera
                    
                    if (controllerList[carTransformList.IndexOf(carTransform)].isPlayer) // Si el coche que ha terminado la carrera es el jugador
                    {
                        print("HAS QUEDADO EL " + positions); // Se muestra la posición en que ha terminado
                    }
                }
                    
            }
            print("Lessgooo " + carTransformList.IndexOf(carTransform));
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;
            
        }
        else
        {
            print("whutttt " + carTransformList.IndexOf(carTransform));
        }
    }
}
