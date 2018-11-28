using UnityEngine;

public class Revolve : MonoBehaviour
{
    public float revolveSpeed = 0f;

    private void Update()
    {
        transform.Rotate(0f, Time.deltaTime * revolveSpeed, 0f);
    }
}
