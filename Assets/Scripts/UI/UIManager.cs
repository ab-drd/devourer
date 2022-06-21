using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Transformation")]
    [SerializeField] private Transform transformationCooldownBar;
    [SerializeField] private Transformation player;
    [SerializeField] private Image transformationIconHolder;

    [Header("Transformation bar colors")]
    [SerializeField] private Color fullColor;
    [SerializeField] private Color emptyColor;

    [Header("Health")]
    [SerializeField] private GameObject healthBar;

    [Header ("Hurt")]
    [SerializeField] private Image hurtOverlay;
    [SerializeField] private float hurtAlphaStart;
    [SerializeField] private float hurtAlphaGoal;
    [SerializeField] private float hurtSpeed;

    [Header ("Heal")]
    [SerializeField] private Image healOverlay;
    [SerializeField] private float healAlpha;
    [SerializeField] private float healSpeed;

    [Header ("Death")]
    [SerializeField] private float deathWaitPeriod;
    [SerializeField] public GameObject deathOverlay;
    [SerializeField] private float deathAlpha;

    [Header("Burst")]
    [SerializeField] private GameObject burstBar;
    [SerializeField] private GameObject burstBarHolder;

    private int active;
    private Image transformationCooldownImage;
    private List<GameObject> playerForms;
    private List<Image> formIcons;

    private void Awake()
    {
        Cursor.visible = false;
        transformationCooldownImage = transformationCooldownBar.GetComponentInChildren<Image>();
        SetImages();
        deathOverlay.SetActive(false);
    }

    private void Start()
    {
        SetActiveForm();
    }

    private void SetImages()
    {
        playerForms = new List<GameObject>();
        formIcons = new List<Image>();

        for (var i = 0; i < player.unlockedFormCount; i++)
        {
            playerForms.Add(player.transform.GetChild(i).gameObject);
            formIcons.Add(transformationIconHolder.transform.GetChild(i).GetComponent<Image>());

            formIcons[i].gameObject.SetActive(true);
        }
    }

    public void SetTransformationIconHolderSize()
    {
        transformationIconHolder.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, player.unlockedFormCount * 50);
        transformationIconHolder.transform.localPosition = new Vector3(0, -player.unlockedFormCount * 25, 0);
    }

    private void SetActiveForm()
    {
        for (int i = 0; i < playerForms.Count; i++)
        {
            if (!playerForms[i].activeInHierarchy)
            {
                formIcons[i].color = Color.black;

            }
        }
    }

    public void RefreshTransformationIcons()
    {
        playerForms.Add(player.transform.GetChild(player.unlockedFormCount - 1).gameObject);
        formIcons.Add(transformationIconHolder.transform.GetChild(player.unlockedFormCount - 1).GetComponent<Image>());
        formIcons[player.unlockedFormCount - 1].gameObject.SetActive(true);
        SetActiveForm();
        SetTransformationIconHolderSize();
    }

    public void ChangeActiveForm(int newActive)
    {
        formIcons[active].color = Color.black;
        formIcons[newActive].color = Color.white;
        active = newActive;
    }

    public void SetTransformationCooldownScale(float scale)
    {
        scale = Mathf.Clamp(scale, 0, 1);
        transformationCooldownBar.localScale = new Vector3(scale, 1, 1);

        transformationCooldownImage.color = new Color(fullColor.r * scale + emptyColor.r * (1 - scale), fullColor.g * scale + emptyColor.g * (1 - scale), fullColor.b * scale + emptyColor.b * (1 - scale));
    }

    public void SetHealthValue(float scale)
    {
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(scale, 0, 1), 1, 1);
    }

    public void SetBurstValue(float scale)
    {
        burstBar.transform.localScale = new Vector3(1, Mathf.Clamp(scale, 0, 1), 1);
    }

    public void DeactivateBurstBar()
    {
        burstBarHolder.SetActive(false);
    }

    public void ActivateBurstBar()
    {
        burstBarHolder.SetActive(true);
    }

    public IEnumerator HurtOverlayFade(float duration)
    {
        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, Mathf.Clamp(hurtAlphaStart, 0, 1));
        float timer = 0;
        while (timer < duration)
        {
            if (hurtOverlay.color.a > 0)
                hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, Mathf.Lerp(hurtOverlay.color.a, hurtAlphaGoal, hurtSpeed));

            timer += Time.deltaTime;
            yield return 0;
        }

        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, 0f);
        yield return 0;
    }

    public void HurtOverlayDeactivate()
    {
        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, 0f);
    }

    public IEnumerator HealFlash()
    {
        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, Mathf.Clamp(healAlpha, 0, 1));
        while (healOverlay.color.a > 0)
        {
            healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, Mathf.Lerp(healOverlay.color.a, 0f, healSpeed));

            yield return 0;
        }

        healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, 0f);
        yield return 0;
    }

    public IEnumerator ActivateDeathScreen()
    {
        GetComponent<PauseMenu>().escape.Disable();
        
        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, Mathf.Clamp(deathAlpha, 0, 1));
        yield return new WaitForSeconds(deathWaitPeriod);
        hurtOverlay.color = new Color(hurtOverlay.color.r, hurtOverlay.color.g, hurtOverlay.color.b, 0);

        GetComponent<PauseMenu>().Pause();
        deathOverlay.SetActive(true);

        yield return 0;
    }
}
