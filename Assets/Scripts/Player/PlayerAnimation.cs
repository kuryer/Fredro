using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    string currentState;

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        Debug.Log("Changed to" + newState);
        animator.Play(newState);
        currentState = newState;
    }
}
