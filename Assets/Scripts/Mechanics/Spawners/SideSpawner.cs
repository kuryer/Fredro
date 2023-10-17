using System.Collections;
using UnityEngine;

public class SideSpawner : MonoBehaviour
{
    bool canSpawn;
    [SerializeField] GameObject Enemy;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float rayLength;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float TimeToSpawn;

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(TimeToSpawn);
        }
    }

    private void FixedUpdate()
    {
       canSpawn = Physics2D.Raycast(transform.position + rayOffset, Vector3.left, rayLength, playerLayer) ||
            Physics2D.Raycast(transform.position - rayOffset, Vector3.left, rayLength, playerLayer);
    }

    public void Spawn()
    {
        if (canSpawn)
        {
            Instantiate(Enemy, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + rayOffset, Vector3.left * rayLength);
        Gizmos.DrawRay(transform.position - rayOffset, Vector3.left * rayLength);
    }

}
