using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField] private GameObject itemCheck;
    [SerializeField] private float fadeSpeed;

    private Text text;
    private bool coroutineStarted;

    private void Awake()
    {
        coroutineStarted = false;
        text = GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
    private void Update()
    {
        if (!coroutineStarted && !itemCheck.activeInHierarchy)
            StartCoroutine(IncreaseToFullAlpha(fadeSpeed, text));            
    }
    public IEnumerator IncreaseToFullAlpha(float fadeSpeed, Text text)
    {
        coroutineStarted = true;
        while (text.color.a < 1f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.fixedDeltaTime / fadeSpeed));
            yield return null;
        }
    }
}