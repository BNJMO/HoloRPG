using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.LookAt(target);
    }
}
