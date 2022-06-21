using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAndAppear : MonoBehaviour
{
    [SerializeField] private GameObject[] appearingObjects;

    public void MakeObjectsAppear()
    {
        for (var i = 0; i < appearingObjects.Length; i++)
        {
            if (appearingObjects[i].GetComponent<TextFade>())
            {
                StartCoroutine(appearingObjects[i].GetComponent<TextFade>().IncreaseToFullAlpha());
            }
            else
            {
                appearingObjects[i].gameObject.SetActive(true);
            }
        }
    }
}
