using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Vector3 rayOffset;
    [SerializeField] float rayLength;
    [SerializeField] Vector3 groundOffset;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    bool canHit;
    bool isGrounded;

    private void Awake()
    {
        canHit = true;
        Destroy(transform.parent.gameObject, 12f);
        Physics2D.IgnoreLayerCollision(7, 8, true);
    }
    private void Update()
    {
        if (isGrounded)
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            transform.position += Vector3.down * movementSpeed * 2 * Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayOffset, Vector3.right, rayLength, playerLayer);

        isGrounded = Physics2D.Raycast(transform.position - groundOffset, Vector3.down * rayLength, groundLayer);

        if(hit.collider != null && hit.collider.CompareTag("Player") && canHit)
        {
            PlayerHealth healthScript = hit.collider.gameObject.GetComponent<PlayerHealth>();
            healthScript.PlayerGotHit();
            canHit = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + rayOffset, Vector3.right * rayLength);
        Gizmos.DrawRay(transform.position + groundOffset, Vector3.down * rayLength);
    }
}
