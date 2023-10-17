using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] int actualLevel;
    [Header("Spawners")]
    [SerializeField] UpperSpawner upperSpawnerScript;
    [SerializeField] List<SideSpawner> sideSpawners;
    [SerializeField] GameObject SpawnersCollection;

    [Header("Grid")]
    [SerializeField] GridGenerator Grid;

    [Header("UI")]
    [SerializeField] GameObject ArrowDisplay;

    [Header("Camera Follow")]
    [SerializeField] public GameObject cameraFollow;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    public int GenerateLevel(int lastLadderPlace)
    {
        Grid.DeleteGrid();
        return Grid.GenerateGrid(lastLadderPlace);
    }

    public void LevelFinished()
    {
        Vector3 arrowPos = new Vector3 (transform.position.x + 20, transform.position.y + 85, 0);
        Instantiate(ArrowDisplay, arrowPos, Quaternion.identity);
        Grid.ChangeLadderState();
    }

    public void ActivateLevel()
    {
        SpawnersCollection.SetActive(true);
        if(actualLevel > 0)
        {
            Debug.Log("Upgraded");
            if(upperSpawnerScript.TimeToSpawn > 1f)
                upperSpawnerScript.TimeToSpawn -= .5f;
            if(upperSpawnerScript.spawnedPropAmount < 3)
                upperSpawnerScript.spawnedPropAmount++;
        }
        actualLevel++;
    }

    public void DeactivateLevel()
    {
        SpawnersCollection.SetActive(false);
    }

    public void MoveUp()
    {
        Debug.Log(transform.position);
        transform.position += new Vector3(0, 360, 0);
        Debug.Log(transform.position);
    }
}