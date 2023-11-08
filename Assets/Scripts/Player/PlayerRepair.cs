using System.Collections;
using UnityEngine;

public class PlayerRepair : MonoBehaviour
{
    [SerializeField] bool canRepair;
    [SerializeField] Tile repairedTile;
    [SerializeField] float fixDelay;
    PlayerMovement movementScript;
    PlayerSound sound;
    Animator animator;
    public enum State
    {
        repairing,
        not
    }
    State actualState;
    private void Awake()
    {
        sound = GetComponent<PlayerSound>();
        actualState = State.not;
        movementScript = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(movementScript.isHit && actualState == State.repairing)
        {
            //movementScript.SetCanMove(true);
            movementScript.setIsRepairing(false);
            actualState = State.not;
        }

        if (Input.GetKeyUp(KeyCode.E) && actualState == State.repairing)
        {
            movementScript.ChangeAnimationState("Player_Idle");
            movementScript.setIsRepairing(false);
            actualState = State.not;
        }

        if (!canRepair)
            return;


        if (Input.GetKeyDown(KeyCode.E) && actualState == State.not && !movementScript.isHit)
        {
            movementScript.ChangeAnimationState("Player_Repairing");
            movementScript.setIsRepairing(true);
            actualState = State.repairing;
        }

    }

    public void SetCanRepairTile(Tile tile, bool state)
    {
        canRepair = state;
        repairedTile = tile;
    }

    public void RepairTile()
    {
        sound.AudioRepair();
        StartCoroutine(FixTile());
    }
    IEnumerator FixTile()
    {
        yield return new WaitForSeconds(fixDelay);
        repairedTile.RepairTile();
        movementScript.setIsRepairing(false);
        animator.Play("Player_Idle");
        canRepair = false;
    }
}
