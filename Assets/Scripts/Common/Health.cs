using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maximumHealth;
    [SerializeField] private GameObject[] forms;

    [Header("Shake")]
    [SerializeField] private CameraShaker cameraToShake;
    [SerializeField] private float shakeMagnitude;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityPeriod;

    [Header("Death")]
    [SerializeField] private float deathShakePeriod;

    [Header("UI Manager")]
    [SerializeField] private UIManager uiManager;

    private float currentHealth;
    public float healthAtLastCheckpoint;
    private bool invulnerable;

    private Animator anglerAnim;

    private void Awake()
    {
        currentHealth = maximumHealth;
        healthAtLastCheckpoint = maximumHealth;
        anglerAnim = forms[1].GetComponentInChildren<Animator>();
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

        SetUIHealth();
    }

    public void Heal(float _healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, maximumHealth);
        SetUIHealth();
        StartCoroutine(uiManager.HealFlash());
    }

    private void Die()
    {
        if (forms[0].activeInHierarchy)
        {
            GetComponentInChildren<SphereMovement>().enabled = false;
            GetComponentInChildren<CircleCollider2D>().enabled = false;
        }

        if (forms[1].activeInHierarchy)
        {
            anglerAnim.SetBool("isDead", true);
            GetComponentInChildren<AnglerfishMovement>().enabled = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
        }

        StartCoroutine(cameraToShake.CameraShake(deathShakePeriod, 2 * shakeMagnitude, 1.001f));
        StartCoroutine(uiManager.GetComponent<DeathMenu>().ActivateDeathScreen());

        GetComponentInChildren<Rigidbody2D>().Sleep();
        anglerAnim.enabled = false;
        GetComponent<Transformation>().enabled = false;
    }

    private void Hurt()
    {
        StartCoroutine(cameraToShake.CameraShake(invulnerabilityPeriod / 4, shakeMagnitude, 1f));
        StartCoroutine(uiManager.HurtOverlayFade(invulnerabilityPeriod));
        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;

        if (forms[1].activeInHierarchy)
            anglerAnim.SetTrigger("hurt");

        yield return new WaitForSeconds(invulnerabilityPeriod);
        invulnerable = false;
        yield return 0;
    }

    private void SetUIHealth()
    {
        uiManager.SetHealthValue(currentHealth / maximumHealth);
    }

    public void UpdateCheckpointHealth()
    {
        healthAtLastCheckpoint = currentHealth;
    }

    public void LoadCheckpointHealth()
    {
        currentHealth = healthAtLastCheckpoint;
        SetUIHealth();
    }
    public void FreezeMovement()
    {
        foreach (GameObject form in forms)
        {
            form.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            form.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        }
    }

    public void RespawnActivators()
    {
        if (forms[0].activeInHierarchy)
        {
            GetComponentInChildren<SphereMovement>().enabled = true;
            GetComponentInChildren<CircleCollider2D>().enabled = true;
        }

        if (forms[1].activeInHierarchy)
        {
            anglerAnim.SetBool("isDead", false);
            GetComponentInChildren<AnglerfishMovement>().enabled = true;
            GetComponentInChildren<PolygonCollider2D>().enabled = true;
        }

        GetComponentInChildren<Rigidbody2D>().WakeUp();
        anglerAnim.enabled = true;
        GetComponent<Transformation>().enabled = true;
    }

    
}
