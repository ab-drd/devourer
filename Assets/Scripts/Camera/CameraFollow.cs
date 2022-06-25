using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float maxTeleportCameraDistance;
    private float cameraZ;
    private int childIndex;
    private void Awake()
    {
        childIndex = 0;
        cameraZ = transform.position.z;
    }
    private void LateUpdate()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, player.GetChild(childIndex).position)) < maxTeleportCameraDistance)
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.GetChild(childIndex).position.x, player.GetChild(childIndex).position.y, cameraZ), 0.5f);
        else
            transform.position = new Vector3(player.GetChild(childIndex).position.x, player.GetChild(childIndex).position.y, cameraZ);
    }

    public void ChangeFollowTarget(int index)
    {
        childIndex = index;
    }
}
