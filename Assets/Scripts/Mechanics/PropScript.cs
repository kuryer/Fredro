using UnityEngine;

public class PropScript : MonoBehaviour
{
    [SerializeField] bool isRotating;
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
        Debug.Log("dotkn¹³");
    }
}
