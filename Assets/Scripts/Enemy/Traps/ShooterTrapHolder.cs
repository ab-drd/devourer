using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrapHolder : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] harpoons;

    private float cooldownTimer;

    private void Awake()
    {
        cooldownTimer = 0;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        cooldownTimer = 0;

        harpoons[FindHarpoon()].transform.position = firepoint.position;
        harpoons[FindHarpoon()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindHarpoon()
    {
        for (int i = 0; i < harpoons.Length; i++)
        {
            if (!harpoons[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
