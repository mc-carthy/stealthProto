using UnityEngine;

public class Player : MonoBehaviour {

	private float moveSpeed = 7f;
    private float smoothMoveTime = 0.1f;
    private float turnSpeed = 8f;
    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private float angle;

    private void Update()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
        transform.eulerAngles = Vector3.up * angle;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World);
    }

}
