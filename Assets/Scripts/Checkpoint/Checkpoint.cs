using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform appearPoint;
    [SerializeField] private Transform maxPoint;
    [SerializeField] private Transform flag;
    [SerializeField] private float speed;

    public bool activated;
    private CheckpointChecker checkpointHolder;

    private void Awake()
    {
        checkpointHolder = GetComponentInParent<CheckpointChecker>();
        flag.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (activated)
            flag.position = Vector3.Lerp(flag.position, maxPoint.position, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            activated = true;
            collision.gameObject.GetComponentInParent<Health>().UpdateCheckpointHealth();

            flag.gameObject.SetActive(true);
            flag.position = appearPoint.position;

            checkpointHolder.IncrementCounter(GetComponent<Checkpoint>());
        }
    }
}
