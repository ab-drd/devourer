using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
    }
}
