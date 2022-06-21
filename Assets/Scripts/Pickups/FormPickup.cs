using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormPickup : MonoBehaviour
{
    [SerializeField] private int formNumber;
    [SerializeField] private FadeAndAppear appearHolder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponentInParent<Transformation>().SetFormCount(formNumber);

            appearHolder.MakeObjectsAppear();

            gameObject.SetActive(false);
        }
    }
}
