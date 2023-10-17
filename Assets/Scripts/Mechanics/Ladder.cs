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
    [SerializeField] bool isFinalLadder;
    [SerializeField] bool isLevelDone;
    [SerializeField]bool isUpside;
    [SerializeField]bool isDownside;
    bool isFinalizeCalled = false;  
    bool isInside;
    PlayerMovement playerMovement;
    GameManager manager;

    private void Awake()
    {
        isFinalLadder = false;
        isLevelDone = false;
        var player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(isFinalLadder && isUpside && isLevelDone && !isFinalizeCalled)
        {
            manager.GoToNextLevel();
            playerMovement.isInTransition = true;
            isFinalizeCalled = true;
        }
    }

    public void SetLevelDone()
    {
        isLevelDone = true;
    }

    public void SetFinalLadder()
    {
        isFinalLadder = true;
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("Game Manager");
        manager = gameManagerObject.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFinalizeCalled && isFinalLadder)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerMovement>(out PlayerMovement Movement);
            {
                playerMovement.SetCanClimb(true, isDownside, isUpside, IsFinalLadderBool());
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
                playerMovement.SetCanClimb(false, isDownside, isUpside, IsFinalLadderBool());
                isInside = false;
            }
        }   
    }

    bool IsFinalLadderBool()
    {
        if (!isFinalLadder)
            return false;
        if(isFinalLadder && !isLevelDone)
            return true;
        return false;
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
