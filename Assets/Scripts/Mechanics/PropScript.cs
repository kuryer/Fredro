using UnityEngine;

public class PropScript : MonoBehaviour
{
    [SerializeField] bool isRotating;
    bool canHit = true;
    [SerializeField] float propLength;
    [SerializeField] float rotationSpeed;
    void Update()
    {
        if(isRotating)
            Rotate();
    }
    void Rotate()
    {
        Vector3 rotationVector = new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        transform.Rotate(rotationVector);
    }

    public float GetLength()
    {
        return propLength;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canHit)
        {
            collision.TryGetComponent<PlayerHealth>(out PlayerHealth Health);
            {
                Debug.Log("Elo");
                Health.PlayerGotHit();
                canHit = false;
            }
        }
    }
}
