using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    private GameManager _GameManager;
    private SpriteRenderer spriteRenderer;
    public Sprite[] spriteObjeto;
    public bool open;
    public GameObject[] lootPrefab;
    private bool gerouLoot;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interacao()
    {
        open = !open;

        switch (open) 
        {
            case true:
                spriteRenderer.sprite = spriteObjeto[1];

                if (!gerouLoot) 
                {
                    StartCoroutine("GerarLoot");
                }
                break;
            case false:
                spriteRenderer.sprite = spriteObjeto[0];
                break;
        }
    }

    IEnumerator GerarLoot() 
    {
        gerouLoot = true;
        int qtdMoedas = Random.Range(1, 5);
        for (int l = 0; l < qtdMoedas; l++)
        {
            int rand = 0, idLoot = 0;
            rand = Random.Range(0, 100);
            if (rand >= 50) 
            {
                idLoot = 1;
            }
            GameObject lootTemp = Instantiate(lootPrefab[idLoot], transform.position, transform.localRotation);
            lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-20, 20), 75));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
