using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        Color currentColor = image.color;
        currentColor.a = 0;
        image.color = currentColor;
        StartCoroutine(FadeInImage());
    }

    private IEnumerator FadeInImage()
    {
        Color currentColor = image.color;
        for (float i = 0; i < 1; i += 0.01f)
        {
            currentColor.a = i;
            image.color = currentColor;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
