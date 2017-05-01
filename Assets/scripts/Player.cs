using UnityEngine;

public class Player : MonoBehaviour {

    public event System.Action OnReachedEndOfLevel;

    private Rigidbody rb;
	private float moveSpeed = 7f;
    private float smoothMoveTime = 0.1f;
    private float turnSpeed = 8f;
    private Vector3 velocity;
    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private float angle;
    private bool disabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
    }

    private void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        float inputMagnitude = inputDirection.magnitude;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "finish")
        {
            Disable();
            if (OnReachedEndOfLevel != null)
            {
                OnReachedEndOfLevel();
            }
        }
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }

    private void Disable()
    {
        disabled = true;
    }
}
