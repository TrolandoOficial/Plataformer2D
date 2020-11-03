using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public string[] tiposDeDano;
    public GameObject[] fxDanoPrefab;
    public GameObject fxMorte;
    public int gold;
    public TextMeshProUGUI goldTXT;

    private void Update()
    {
        goldTXT.text = gold.ToString("N0");
    }
}
