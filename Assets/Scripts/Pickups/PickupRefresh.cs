using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRefresh : MonoBehaviour
{
    private HealthPickup[] healthPickups;
    private void Awake()
    {
        healthPickups = GetComponentsInChildren<HealthPickup>(true);
    }
    public void ReactivateHealthPickups()
    {
        foreach (HealthPickup pickup in healthPickups)
        {
            pickup.gameObject.SetActive(true);
        }
    }
}
