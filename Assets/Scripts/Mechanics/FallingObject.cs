using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float fallingSpeed;
    [SerializeField] FallingObjectSO vars;
    private void Update()
    {
        Falling();
    }

    void Falling()
    {
        transform.Translate(transform.up * -1 * vars.fallingSpeed * Time.deltaTime);
    }
}
