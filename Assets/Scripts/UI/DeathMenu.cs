using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] private float deathWaitPeriod;
    [SerializeField] public GameObject deathOverlay;
    [SerializeField] private float deathAlpha;
    [SerializeField] private Image hurtOverlay;

    private void Awake()
    {
        deathOverlay.SetActive(false);
    }
    public IEnumerator ActivateDeathScreen()
    {
        GetComponent<PauseMenu>().escape.Disable();

        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, Mathf.Clamp(deathAlpha, 0, 1));
        yield return new WaitForSeconds(deathWaitPeriod);
        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, 0);

        GetComponent<UIManager>().Pause();
        deathOverlay.SetActive(true);

        yield return 0;
    }
}
