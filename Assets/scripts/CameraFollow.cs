using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Transform player;
    private float cameraHeight;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraHeight = transform.position.y;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, cameraHeight, player.position.z);
    }

}
