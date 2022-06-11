using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maximumHealth;
    [SerializeField] private GameObject[] forms;

    [Header("Shake")]
    [SerializeField] private CameraFollow cameraToShake;
    [SerializeField] private float shakeMagnitude;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityPeriod;

    [Header("UI Manager")]
    [SerializeField] private UIManager uiManager;

    private float currentHealth;
    private bool invulnerable;

    private Animator anim;
    private void Awake()
    {
        currentHealth = maximumHealth;
        anim = forms[1].GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            Hurt();
        }

        uiManager.SetHealthValue(currentHealth / maximumHealth);
    }

    public void Heal(float _healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, maximumHealth);
        uiManager.SetHealthValue(currentHealth / maximumHealth);
        StartCoroutine(uiManager.HealFlash());
    }

    public void Die()
    {
        Debug.Log("Dead!");
        if (forms[0].activeInHierarchy)
        {
            GetComponentInChildren<SphereMovement>().enabled = false;
            GetComponentInChildren<CircleCollider2D>().enabled = false;
        }

        if (forms[1].activeInHierarchy)
        {
            anim.SetBool("isDead", true);
            GetComponentInChildren<AnglerfishMovement>().enabled = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
        }

        StartCoroutine(uiManager.ActivateDeathScreen());
        GetComponentInChildren<Rigidbody2D>().Sleep();
        GetComponent<Transformation>().enabled = false;
    }

    public void Hurt()
    {
        Debug.Log("Hurt!");
        StartCoroutine(cameraToShake.CameraShake(invulnerabilityPeriod, shakeMagnitude));
        StartCoroutine(uiManager.HurtOverlayFade(invulnerabilityPeriod));
        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;

        if (forms[1].activeInHierarchy)
            anim.SetTrigger("hurt");

        yield return new WaitForSeconds(invulnerabilityPeriod);
        invulnerable = false;
        yield return 0;
    }
}
