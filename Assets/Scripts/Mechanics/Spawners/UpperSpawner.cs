using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperSpawner : MonoBehaviour
{
    [Header("Spawn Variables")]
    [Range(2f, 200f)]
    [SerializeField] float SpawnAreaDistance;
    [SerializeField] float deletionAreaDistance;
    [Range(0f, 200f)]
    [SerializeField] float gizmosRayLength;
    [SerializeField] public float TimeToSpawn;
    [SerializeField] public int spawnedPropAmount;
    [Header("Delete Variables")]
    [SerializeField] LayerMask propLayer;
    [Header("Prop Variables")]
    [SerializeField] List<GameObject> props;
    [SerializeField] List<GameObject> spawnedProps;
    [SerializeField] int lastPropIndex;
    float lastPropLength;
    float lastPropPositionX;
    [SerializeField] bool isDisabled;
    [SerializeField] int levelNum;
    [Header("Warning")]
    [SerializeField] GameObject warning;

    private void OnEnable()
    {
        if(levelNum == 1 && !isDisabled)
        {
            StartCoroutine(SpawnProp());
            Debug.Log("started from onEnable");
        }   
    }

    private void OnDisable()
    {
        for(int i =0; i < (spawnedProps.Count-1); i++)
        {
            Destroy(spawnedProps[i].gameObject);
            spawnedProps.Remove(spawnedProps[i]);
        }
        StopAllCoroutines();
        isDisabled = true;
    }

    public void Enable()
    {
        if (isDisabled)
        {
            Debug.Log("started from Enable");
            StartCoroutine(SpawnProp());
            isDisabled = false;
        }
    }

    public void Disable()
    {
        for (int i = 0; i < (spawnedProps.Count); )
        {
            Destroy(spawnedProps[i].gameObject);
            spawnedProps.Remove(spawnedProps[i]);
        }
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        Vector2 origin = new Vector2(transform.position.x - SpawnAreaDistance, transform.position.y - gizmosRayLength);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.left, deletionAreaDistance, propLayer);

        if (hit.collider != null && hit.collider.CompareTag("Prop"))
        {
            Destroy(hit.collider.gameObject.transform.parent.gameObject);
        }
    }

    IEnumerator SpawnProp()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(TimeToSpawn);
        }
    }

    void Spawn()
    {
        for(int i = 0; i < spawnedPropAmount; i++)
        {
            Vector3 spawnPos = GetPropSpawnPosition();
            Vector3 offset = new Vector3(0, -65, 0);
            Instantiate(warning, spawnPos + offset, Quaternion.identity);
            GameObject prop = Instantiate(props[GetPropIndex()], spawnPos, Quaternion.identity);
            PropScript script = prop.GetComponentInChildren<PropScript>();
            lastPropLength = script.GetLength();
            spawnedProps.Add(prop);
        }
    }

    int GetPropIndex()
    {
        int index = Random.Range(0, props.Count);
        if (index == lastPropIndex)
            return GetPropIndex();
        lastPropIndex = index;
        return index;
    }

    Vector3 GetPropSpawnPosition()
    {
        float randomX = Random.Range(transform.position.x - SpawnAreaDistance, transform.position.x + SpawnAreaDistance);

        if (IsGeneratedPositionFree(randomX))
        {
            Vector3 newPos = new Vector3(randomX, transform.position.y, transform.position.z);
            lastPropPositionX = randomX;
            return newPos;
        }
        else
        {
            return GetPropSpawnPosition();
        }
    }
    bool IsGeneratedPositionFree(float positionToCheck)
    {
        if (lastPropPositionX == 0)
        {
            return true;
        }
        if (positionToCheck > lastPropPositionX + (lastPropLength / 2) || positionToCheck < lastPropPositionX - (lastPropLength / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        //////////////////////////
        //Draws Leaves Spawn Range
        //////////////////////////
        Gizmos.color = Color.cyan;
        Vector3 cubeSize = new Vector3(SpawnAreaDistance * 2, 1f, 0f);
        Gizmos.DrawWireCube(transform.position, cubeSize);

        //////////////////
        // Draws Side Rays
        //////////////////
        Vector3 rayPos = new Vector3(cubeSize.x / 2, 0f, 0f);
        Vector3 rayLength = new Vector3(0f, rayPos.y - gizmosRayLength, 0f);
        Gizmos.DrawLine(transform.position - rayPos, transform.position - rayPos + rayLength);
        Gizmos.DrawLine(transform.position + rayPos, transform.position + rayPos + rayLength);

        ////////////////////////
        //Draws The Deleting Ray
        ////////////////////////
        Gizmos.color = Color.red;
        Vector3 from = new Vector3(transform.position.x + deletionAreaDistance, transform.position.y - gizmosRayLength, 0);
        Vector3 to = new Vector3(transform.position.x - deletionAreaDistance, transform.position.y - gizmosRayLength, 0);
        Gizmos.DrawLine(from, to);
    }

    #endregion
}
