using UnityEngine;

public class TeleportToTarget : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    [SerializeField] private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            TeleportPlayer(teleportPosition.position, player);
    }

    public void TeleportPlayer(Vector3 _teleportPosition, Transform _player)
    {
        _player.GetComponent<Transformation>().ResetFormPositions();
        _player.transform.position = _teleportPosition;
    }
}
