using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    [SerializeField] private Checkpoint[] checkpoints;

    private int activatedCounter;

    private void Awake()
    {
        activatedCounter = -1;
    }

    public void IncrementCounter(Checkpoint _checkpoint)
    {
        for (var i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == _checkpoint)
            {
                activatedCounter = i;
                break;
            }
        }
    }

    public Vector3 TeleportPosition()
    {
        if (activatedCounter > -1)
            return checkpoints[activatedCounter].transform.position;
        else
            return new Vector3();
    }
}
