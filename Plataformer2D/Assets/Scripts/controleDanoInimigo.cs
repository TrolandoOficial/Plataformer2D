using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controleDanoInimigo : MonoBehaviour
{
    private GameManager _GameManager;
    private playerScript _PlayerScript;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Configuração Resistência/Fraqueza")]
    public float[] ajusteDeDano;
    public bool isLookingLeft;
    public bool playerEsquerda;

    [Header("Configuração de Vida")]
    public int vidaInimigo;
    public int vidaAtualInimigo;
    public GameObject barrasVida;
    public GameObject danoTXTPrefab;
    public Transform hpBar;
    public Color[] characterColor;
    private float percentVida;

    //KNOCKBACK
    [Header("KNOCKBACK")]
    public GameObject knockForcePrefab;
    public Transform knockPosition;
    public float knockX;
    private float kx;

    //HIT
    private bool isHit;
    private bool isDead;

    [Header("Configuração de Chão")]
    public Transform groundCheck;
    public LayerMask whatIsGround;

    [Header("Configuração de Loot")]
    public GameObject[] lootPrefab;


    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        _PlayerScript = FindObjectOfType(typeof(playerScript)) as playerScript;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spriteRenderer.color = characterColor[0];

        barrasVida.SetActive(false);
        vidaAtualInimigo = vidaInimigo;
        hpBar.localScale = new Vector3(1, 1, 1);

        if (isLookingLeft)
        {
            float x = transform.localScale.x;
            x *= -1;
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            barrasVida.transform.localScale = new Vector3(x, barrasVida.transform.localScale.y, barrasVida.transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //VERIFICA SE O PLAYER ESTA A DIREITA OU ESQUERDA DO INIMIGO
        float xPlayer = _PlayerScript.transform.position.x;

        if (xPlayer < transform.position.x)
        {
            playerEsquerda = true;
        }
        else if (xPlayer > transform.position.x)
        {
            playerEsquerda = false;
        }

        if (isLookingLeft && playerEsquerda)
        {
            kx = knockX;
        }
        else if (!isLookingLeft && playerEsquerda)
        {
            kx = knockX * -1;
        }
        else if (isLookingLeft && !playerEsquerda)
        {
            kx = knockX * -1;
        }
        else if (!isLookingLeft && !playerEsquerda)
        {
            kx = knockX;
        }

        knockPosition.localPosition = new Vector3(kx, knockPosition.localPosition.y, 0);

        animator.SetBool("isGrounded", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        armaInfo armaInfo = collision.gameObject.GetComponent<armaInfo>();
        if (isDead) return;
        
        switch (collision.gameObject.tag)
        {
            case "arma":
                if (!isHit)
                {
                    isHit = true;
                    barrasVida.SetActive(true);

                    animator.SetTrigger("hit");

                    float danoArma = Random.Range(armaInfo.minDamage, armaInfo.maxDamage);
                    int tipoDano = armaInfo.typeDamage;

                    float danoTomado = danoArma + (danoArma * (ajusteDeDano[tipoDano]) / 100);

                    vidaAtualInimigo -= Mathf.RoundToInt(danoTomado);

                    percentVida = (float)vidaAtualInimigo / (float)vidaInimigo;

                    if (percentVida < 0) percentVida = 0;

                    hpBar.localScale = new Vector3(percentVida, 1, 1);

                    if (vidaAtualInimigo <= 0)
                    {
                        isDead = true;
                        animator.SetInteger("idAnimation", 3);
                        StartCoroutine("Loot");
                    }

                    GameObject danoTemp = Instantiate(danoTXTPrefab, transform.position, transform.localRotation);
                    danoTemp.GetComponent<TextMesh>().text = Mathf.RoundToInt(danoTomado).ToString();
                    danoTemp.GetComponent<MeshRenderer>().sortingLayerName = "HUD";

                    GameObject fxTemp = Instantiate(_GameManager.fxDanoPrefab[tipoDano], transform.position, transform.localRotation);
                    Destroy(fxTemp, 1f);

                    int tempForcaX = 50;
                    if (!playerEsquerda) tempForcaX *= -1;
                    danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(tempForcaX, 250));
                    Destroy(danoTemp, 1f);

                    GameObject knockTemp = Instantiate(knockForcePrefab, knockPosition.position, knockPosition.localRotation);
                    Destroy(knockTemp, 0.03f);

                    StartCoroutine("Invuneravel");

                }
                break;
        }
    }

    void Flip()
    {
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        barrasVida.transform.localScale = new Vector3(x, barrasVida.transform.localScale.y, barrasVida.transform.localScale.z);
    }

    IEnumerator Loot()
    {
        yield return new WaitForSeconds(1);
        GameObject fxMorte = Instantiate(_GameManager.fxMorte, groundCheck.position, transform.localRotation);
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;

        int qtdMoedas = Random.Range(5, 20);
        for (int l = 0; l < qtdMoedas; l++) 
        {
            int rand = 0, idLoot = 0;
            rand = Random.Range(0, 100);
            if (rand >= 50)
            {
                idLoot = 1;
            }
            GameObject lootTemp = Instantiate(lootPrefab[idLoot], transform.position, transform.localRotation);
            lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2 (Random.Range(-20, 20), 75));
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(fxMorte);
        Destroy(this.gameObject);
    }

    IEnumerator Invuneravel()
    {
        spriteRenderer.color = characterColor[1];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[0];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[1];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[0];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[1];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[0];
        //barrasVida.SetActive(false);
        isHit = false;
    }
}
