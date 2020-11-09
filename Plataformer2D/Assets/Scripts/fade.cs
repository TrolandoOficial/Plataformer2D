using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{
    public GameObject painelFume;
    public Image fume;
    public Color[] corTransicao;
    public float step;

    public void FadeIn() 
    {
        painelFume.SetActive(true);
        StartCoroutine("FadeI");
    }

    public void FadeOut()
    {
        StartCoroutine("FadeO");
    }

    IEnumerator FadeI()
    {
        for (float i = 0; i < 1; i += step) 
        {
            fume.color = Color.Lerp(corTransicao[0], corTransicao[1], i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeO()
    {
        for (float i = 0; i < 1; i += step)
        {
            fume.color = Color.Lerp(corTransicao[1], corTransicao[0], i);
            yield return new WaitForEndOfFrame();
        }

        painelFume.SetActive(false);
    }
}
