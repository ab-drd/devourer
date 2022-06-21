using System.Collections;
using System.Collections.Generic;
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
    public IEnumerator CameraShake(float duration, float magnitude, float reductionRate)
    {
        float elapsed = 0f;
        float multiplier = 1f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude * multiplier;
            float y = Random.Range(-1f, 1f) * magnitude * multiplier;

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + x, transform.position.y + y, cameraZ), 0.5f);

            multiplier /= reductionRate;

            elapsed += Time.deltaTime;
            yield return 0;
        }

        SetPosition();
    }
}
