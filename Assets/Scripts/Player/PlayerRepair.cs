using UnityEngine;

public class PlayerRepair : MonoBehaviour
{
    [SerializeField] bool canRepair;
    [SerializeField] Tile repairedTile;
    PlayerMovement movementScript;
    Animator animator;
    public enum State
    {
        repairing,
        not
    }
    State actualState;
    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && actualState == State.repairing)
        {
            movementScript.ChangeAnimationState("Player_Idle");
            movementScript.SetCanMove(true);
            actualState = State.not;
        }

        if (!canRepair)
            return;


        if (Input.GetKeyDown(KeyCode.E) && actualState == State.not)
        {
            movementScript.ChangeAnimationState("Player_Repairing");
            movementScript.SetCanMove(false);
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
        repairedTile.RepairTile();
        movementScript.SetCanMove(true);
        animator.Play("Player_Idle");
        canRepair = false;
    }
}
