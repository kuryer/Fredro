using UnityEngine;

[CreateAssetMenu(fileName = "Player Variables", menuName = "Scriptable Objects/ Player Variables")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed;
    public float climbingSpeed;
    public float acceleration;
    public float decceleration;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCutMultiplier;
    public float jumpBuffer;

    [Header("Gravities")]
    public float generalGravity;
    public float fallGravity;
}
