using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
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
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.GetChild(childIndex).position.x, player.GetChild(childIndex).position.y, cameraZ), 0.5f);
    }

    public void ChangeFollowTarget(int index)
    {
        childIndex = index;
    }
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        duration = duration / 4;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + x, transform.position.y + y, cameraZ), 0.5f) ;
            elapsed += Time.deltaTime;
            yield return 0;
        }

        SetPosition();
    }
}
