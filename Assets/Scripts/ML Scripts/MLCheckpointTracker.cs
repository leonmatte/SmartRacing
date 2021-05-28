using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MLCheckpointTracker : MonoBehaviour
{
    [SerializeField] private GameObject carParent;
    
    private List<Transform> carTransformList;
    private List<DrivingAgent> agentList;
    private List<MLCheckpoint> checkpointList;
    private List<carControllerVer4> controllerList;
    private List<int> nextCheckpointList;
    private int positions;
    

    void Awake()
    {
        positions = 0;
        
        Transform checkpointsTransform = transform.Find("Checkpoints");
        agentList = carParent.GetComponentsInChildren<DrivingAgent>().ToList();

        BoxCollider[] colliders = carParent.GetComponentsInChildren<BoxCollider>();
        controllerList = carParent.GetComponentsInChildren<carControllerVer4>().ToList();
        carTransformList = new List<Transform>();
        
        foreach (BoxCollider collider in colliders)
        {
            carTransformList.Add(collider.transform);
        }
        
        print("Agent List: " + agentList.Count);
        
        print("Transform List: " + carTransformList.Count);
        
        print("Controller List: " + controllerList.Count);

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
        DrivingAgent agent = null;

        if (carTransformList.IndexOf(carTransform) < agentList.Count)
        {
            agent = agentList[carTransformList.IndexOf(carTransform)];
        }

        
        
        /*if (agent != null && agent.beginningEpisode)
        {
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = 0;
            print("NEW EPISODE " + nextCheckpointList[carTransformList.IndexOf(carTransform)]);
            agent.beginningEpisode = false;
        }*/
        
        int nextCheckpointIndex = nextCheckpointList[carTransformList.IndexOf(carTransform)];
        
        if(checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            if (nextCheckpointIndex == 0) // Si el checkpoint es el primero
            {
                if (controllerList[carTransformList.IndexOf(carTransform)].isPlayer)
                {
                    //  raceValuesController.EndTimer();
                }
                controllerList[carTransformList.IndexOf(carTransform)].lapCounter++; // Siguiente vuelta
                print("Coche: " + carTransformList.IndexOf(carTransform) + ", vuelta: " +
                      controllerList[carTransformList.IndexOf(carTransform)].lapCounter);
                if (controllerList[carTransformList.IndexOf(carTransform)].lapCounter > 3) // Si el coche ha completado las tres vueltas
                {
                    if (carTransformList.IndexOf(carTransform) < agentList.Count)
                    {
                        agentList[carTransformList.IndexOf(carTransform)].enabled = false;
                    }
                    
                    controllerList[carTransformList.IndexOf(carTransform)]
                        .GetInputFromAI(Random.Range(-0.1f, 0.1f), 0, true, true, false, false);
    
                    controllerList[carTransformList.IndexOf(carTransform)].driving =
                            false; // El coche deja de correr
                    
                    
                    positions++; // Otro coche más ha terminado la carrera
                    
                    if (controllerList[carTransformList.IndexOf(carTransform)].isPlayer) // Si el coche que ha terminado la carrera es el jugador
                    {
                        print("HAS QUEDADO EL " + positions); // Se muestra la posición en que ha terminado
                        Time.timeScale = 0f;
                        AudioSource[] audios = FindObjectsOfType<AudioSource>();
                        foreach (AudioSource a in audios)
                        {
                            a.Pause();
                        }
                        Cursor.visible = true;
                        SceneManager.LoadScene(11, LoadSceneMode.Additive);
                        PlayerPrefs.GetInt("posicionJugador", positions); //Mandar la posición del jugador para recoger en otro script
                    }
                }   
            }
            
            print("Lessgooo " + carTransformList.IndexOf(carTransform));
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;

            //Código para añadir recompensas 
            
            /* agent.AddReward(1f);
            if (nextCheckpointIndex == checkpointList.Count)
            {
                agent.AddReward(5f);
                agent.EndEpisode();
            }*/

            // Asignación del transform del siguiente checkpoint para las observaciones del modelo
            
            if (agent != null)
            {
                agent.SetNextCheckpointTransform(checkpointList[nextCheckpointList[carTransformList.IndexOf(carTransform)]].transform);
            }
            
            
        }
        else
        {
            print("whutttt " + carTransformList.IndexOf(carTransform));
            
            // Penalización en caso de pasar por el checkpoint equivocado
            
            /*agent.AddReward(-1f);
            agent.errorCount++;*/
        }
    }

    // Función que no funcionó
    
    /*public void ResetCheckpointIndex(Transform carTransform)
    {
        if (nextCheckpointList != null){
            print("INDEX OF CAR " + carTransformList.IndexOf(carTransform) + " WAS PREVIOUSLY " +
                  nextCheckpointList[carTransformList.IndexOf(carTransform)]);
            
            nextCheckpointList[carTransformList.IndexOf(carTransform)] = 0;
            
            print("INDEX OF CAR " + carTransformList.IndexOf(carTransform) + " CHANGED TO " +
                  nextCheckpointList[carTransformList.IndexOf(carTransform)]);
        } else print("NULL");
    }*/ 
    
}
