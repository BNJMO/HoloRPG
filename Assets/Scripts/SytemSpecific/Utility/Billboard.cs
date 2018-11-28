using UnityEngine;

/// <summary>
/// The Billboard class implements the behaviors needed to keep a GameObject 
/// oriented towards the user.
/// </summary>
public class Billboard : MonoBehaviour
{
    public Transform targetTransform;

    public enum PivotAxis
    {
        // Rotate about all axes.
        Free,
        // Rotate about an individual axis.
        X,
        Y
    }
    /// <summary>
    /// The axis about which the object will rotate.
    /// </summary>
    [Tooltip("Specifies the axis about which the object will rotate (Free rotates about both X and Y).")]
    public PivotAxis pivotAxis = PivotAxis.Free;

    /// <summary>
    /// Overrides the cached value of the GameObject's default rotation.
    /// </summary>
    public Quaternion DefaultRotation { get; private set; }

    [SerializeField]
    private float rotationSpeed = 1;

    private void Awake()
    {
        // Cache the GameObject's default rotation.
        DefaultRotation = transform.rotation;

        if (targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    /// <summary>
    /// The billboard logic is performed in FixedUpdate to update the object
    /// with the player independent of the frame rate.  This allows the object to 
    /// remain correctly rotated even if the frame rate drops.
    /// </summary>
    private void FixedUpdate()
    {
        // Get a Vector that points targetTransform the Camera to the Target.
        if (targetTransform == null)
        {
            return;
        }
        Vector3 directionToTarget = targetTransform.position - transform.position;

        // Adjust for the pivot axis.
        switch (pivotAxis)
        {
            case PivotAxis.X:
                directionToTarget.x = transform.position.x;
                break;

            case PivotAxis.Y:
                directionToTarget.y = transform.position.y;
                break;

            case PivotAxis.Free:
            default:
                // No changes needed.
                break;
        }

        // Interpolate new direction
        if (directionToTarget.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-directionToTarget) * DefaultRotation, Time.deltaTime * rotationSpeed);
        }   
    }

}
