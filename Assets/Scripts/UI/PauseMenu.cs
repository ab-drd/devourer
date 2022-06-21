using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private GameObject player;
    [SerializeField] private CheckpointChecker checkpointHolder;
    [SerializeField] private GameObject healthPickupHolder;
    

    private Controls controls;
    public InputAction escape;
    private bool paused;
    private Animator[] playerAnimators;
    private SphereMovement sphereMovement;
    private TeleportToTarget teleporter;
    private HealthPickup[] healthPickups;

    private void Awake()
    {
        controls = new Controls();
        playerAnimators = player.GetComponentsInChildren<Animator>(true);
        sphereMovement = player.GetComponentInChildren<SphereMovement>(true);
        healthPickups = healthPickupHolder.GetComponentsInChildren<HealthPickup>(true);
        teleporter = GetComponent<TeleportToTarget>();
    }

    private void OnEnable()
    {
        escape = controls.UI.PauseMenu;
        escape.Enable();
    }

    private void Update()
    {
        if (escape.WasPressedThisFrame())
        {
            if (paused)
                UnpauseGame();
            else
                PauseGame();
        }
    }

    public void Pause()
    {
        paused = true;

        foreach (Animator anim in playerAnimators)
        {
            anim.enabled = false;
        }

        sphereMovement.enabled = false;

        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        paused = false;

        foreach (Animator anim in playerAnimators)
        {
            anim.enabled = true;
        }

        sphereMovement.enabled = true;

        Cursor.visible = false;
        Time.timeScale = 1f;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void PauseGame()
    {
        Pause();

        pauseMenuObject.SetActive(true);
    }

    public void UnpauseGame()
    {
        Unpause();

        pauseMenuObject.SetActive(false);
    }

    public void Respawn()
    {
        GetComponent<PauseMenu>().escape.Enable();
        GetComponent<UIManager>().deathOverlay.SetActive(false);
        player.GetComponent<Health>().RespawnActivators();
        LoadCheckpoint();
    }

    public void LoadCheckpoint()
    {
        GetComponent<UIManager>().HurtOverlayDeactivate();
        ReactivateHealthPickups();

        teleporter.TeleportPlayer(checkpointHolder.TeleportPosition(), player.transform);
        player.GetComponent<Health>().FreezeMovement();
        player.GetComponent<Health>().SetCheckpointHealth();
        UnpauseGame();
    }

    private void ReactivateHealthPickups()
    {
        foreach (HealthPickup pickup in healthPickups)
        {
            pickup.gameObject.SetActive(true);
        }
    }
}
