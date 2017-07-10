using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CharacterController myCharacterController;

    [SerializeField]
    private float moveSpeed = 1;

    [SerializeField]
    private float rotateSpeed = 1;

    private bool isInEditor = false;

    private void Awake()
    {
        myCharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        isInEditor = true;
#endif

        // disable camera controller if not in editor
        if (!isInEditor)
        {
            myCharacterController.enabled = false;
            enabled = false;
        }
    }

    private void Update()
    {
        // movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementHorizontal = transform.right * horizontalInput * moveSpeed * Time.deltaTime;
        Vector3 movementVertical = transform.forward * verticalInput * moveSpeed * Time.deltaTime;

        myCharacterController.Move(movementHorizontal + movementVertical);


        // rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal2") * rotateSpeed * Time.deltaTime);
        transform.Rotate(Vector3.left * Input.GetAxis("Vertical2") * rotateSpeed * Time.deltaTime);

    }
}
