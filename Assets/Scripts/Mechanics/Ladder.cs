using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Vector3 Offset;
    [SerializeField] Vector3 UpsideOffset;
    [SerializeField] Vector3 Distance;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rayLength;
    [SerializeField]bool isUpside;
    [SerializeField]bool isDownside;
    bool isInside;
    PlayerMovement playerMovement;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerMovement>(out PlayerMovement Movement);
            {
                Debug.Log("Elo");
                playerMovement.SetCanClimb(true, isDownside, isUpside);
                isInside = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerMovement>(out PlayerMovement Movement);
            {
                playerMovement.SetCanClimb(false, isDownside, isUpside);
                isInside = false;
            }
        }
    }

    private void FixedUpdate()
    {
        isDownside = Physics2D.Raycast(transform.position + Offset, Vector2.right, rayLength, playerLayer) ||
            Physics2D.Raycast((transform.position + Offset) - Distance, Vector2.right, rayLength, playerLayer) ||
            Physics2D.Raycast((transform.position + Offset) + Distance, Vector2.right, rayLength, playerLayer);

        isUpside = Physics2D.Raycast(transform.position + UpsideOffset, Vector2.right, rayLength, playerLayer);

        if (!isInside)
            return;

        playerMovement.SetClimbingBools(isDownside, isUpside);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + Offset, Vector3.right * rayLength);
        Gizmos.DrawRay(transform.position + UpsideOffset, Vector3.right * rayLength);
        Gizmos.DrawRay((transform.position + Offset) + Distance, Vector3.right * rayLength);
        Gizmos.DrawRay((transform.position + Offset) - Distance, Vector3.right * rayLength);
    }
}
