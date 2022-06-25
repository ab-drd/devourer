using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private float cameraZ;

    private void Awake()
    {
        cameraZ = transform.position.z;
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
    }
}
