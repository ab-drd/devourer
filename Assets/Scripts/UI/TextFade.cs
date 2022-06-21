using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    public IEnumerator IncreaseToFullAlpha()
    {
        while (text.color.a < 1f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.fixedDeltaTime / fadeSpeed));
            yield return null;
        }

        GetComponent<TextFade>().enabled = false;
        yield return null;
    }
}