using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    private GameManager _GameManager;

    public int valor;

    private void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    public void Coletar() 
    {
        _GameManager.gold += valor;
        Destroy(this.gameObject);
    }
}
