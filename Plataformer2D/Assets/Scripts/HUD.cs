using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private playerScript _PlayerScript;
    public Image[] hpBar;
    public Sprite half, full;

    void Start()
    {
        _PlayerScript = FindObjectOfType(typeof(playerScript)) as playerScript;
    }

    private void Update()
    {
        ControleBarraVida();
    }

    void ControleBarraVida()
    {
        float percentVida = (float)_PlayerScript.playerCurrentHP / (float)_PlayerScript.playerMaxHP;

        foreach (Image img in hpBar) 
        {
            img.enabled = true;
            img.sprite = full;
        }

        if (percentVida == 1)
        {

        }
        else if (percentVida >= 0.9f)
        {
            hpBar[4].sprite = half;
        }
        else if (percentVida >= 0.8f)
        {
            hpBar[4].enabled = false;
        }
        else if (percentVida >= 0.7f)
        {
            hpBar[4].enabled = false;
            hpBar[3].sprite = half;
        }
        else if (percentVida >= 0.6f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
        }
        else if (percentVida >= 0.5f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].sprite = half;
        }
        else if (percentVida >= 0.4f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].enabled = false;
        }
        else if (percentVida >= 0.3f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].enabled = false;
            hpBar[1].sprite = half;
        }
        else if (percentVida >= 0.2f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].enabled = false;
            hpBar[1].enabled = false;
        }
        else if (percentVida >= 0.01f)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].enabled = false;
            hpBar[1].enabled = false;
            hpBar[0].sprite = half;
        }
        else if (percentVida <= 0)
        {
            hpBar[4].enabled = false;
            hpBar[3].enabled = false;
            hpBar[2].enabled = false;
            hpBar[1].enabled = false;
            hpBar[0].enabled = false;
        }
    }

}
