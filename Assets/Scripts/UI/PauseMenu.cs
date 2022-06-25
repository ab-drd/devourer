using UnityEngine;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private Health player;
    [SerializeField] private CheckpointChecker checkpointHolder;
    [SerializeField] private PickupRefresh healthPickupHolder;
    
    private Controls controls;
    public InputAction escape;
    private TeleportToTarget teleporter;

    private DeathMenu deathMenu;
    private UIManager uIManager;

    private void Awake()
    {
        controls = new Controls();
        
        teleporter = GetComponent<TeleportToTarget>();
        deathMenu = GetComponent<DeathMenu>();
        uIManager = GetComponent<UIManager>();
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
            if (uIManager.IsPaused())
                UnpauseGame();
            else
                PauseGame();
        }
    }

    public void Respawn()
    {
        escape.Enable();
        deathMenu.deathOverlay.SetActive(false);
        player.RespawnActivators();
        LoadCheckpoint();
    }

    public void LoadCheckpoint()
    {
        uIManager.HurtOverlayDeactivate();
        healthPickupHolder.ReactivateHealthPickups();

        teleporter.TeleportPlayer(checkpointHolder.TeleportPosition(), player.transform);
        player.FreezeMovement();
        player.LoadCheckpointHealth();
        UnpauseGame();
    }

    public void PauseGame()
    {
        uIManager.Pause();

        if (pauseMenuObject.activeInHierarchy == false)
            pauseMenuObject.SetActive(true);
    }

    public void UnpauseGame()
    {
        uIManager.Unpause();

        if (pauseMenuObject.activeInHierarchy == true)
            pauseMenuObject.SetActive(false);
    }
}
