using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    private playerScript _PlayerScript;
    public Transform destino;

    public bool escuro;
    public Material luz2D, padrao2D;

    private fade fade;

    private void Start()
    {
        _PlayerScript = FindObjectOfType(typeof(playerScript)) as playerScript;
        fade = FindObjectOfType(typeof(fade)) as fade;
    }
    public void Interacao()
    {
        StartCoroutine("AcionarPorta");
    }

    IEnumerator AcionarPorta()
    {
        fade.FadeIn();
        yield return new WaitWhile(() => fade.fume.color.a < 0.9f);
        _PlayerScript.gameObject.SetActive(false);
        switch (escuro) 
        {
            case true:
                _PlayerScript.ChangeMaterial(luz2D);
                break;

            case false:
                _PlayerScript.ChangeMaterial(padrao2D);
                break;
        }
        _PlayerScript.transform.position = destino.position;
        yield return new WaitForSeconds(0.3f);
        _PlayerScript.gameObject.SetActive(true);
        fade.FadeOut();
    }
}
