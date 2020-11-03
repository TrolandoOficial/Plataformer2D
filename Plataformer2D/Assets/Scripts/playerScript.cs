using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private GameManager _GameManager;

    private Animator playerAnimator;
    private Rigidbody2D playerRB;

    public Transform groundCheck;
    public Transform hand;
    public LayerMask whatIsGround;
    public LayerMask interacao;

    public int playerMaxHP, playerCurrentHP;

    public float playerSpeed;
    public float jumpForce;
    public bool lookingLeft;
    public bool isGrounded;
    public bool isAttacking;
    public int idAnimation;
    public Collider2D standing, crouching;
    public GameObject objetoInteracao;

    private float h, v;
    private Vector3 dir = Vector3.right;
    public GameObject balaoAlerta;

    //SISTEMA DE ARMAS
    public GameObject[] armas;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();

        playerCurrentHP = playerMaxHP;

        foreach (GameObject o in armas)
        {
            o.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h > 0 && lookingLeft && !isAttacking) Flip();
        else if (h < 0 && !lookingLeft && !isAttacking) Flip();

        if (v < 0) 
        {
            idAnimation = 2;
            if (isGrounded) h = 0;
        }
        else if (h != 0) idAnimation = 1;
        else idAnimation = 0;

        if (Input.GetButtonDown("Fire1") && v >= 0 && !isAttacking && objetoInteracao == null) playerAnimator.SetTrigger("attack");

        if (Input.GetButtonDown("Fire1") && v >= 0 && !isAttacking && objetoInteracao != null) objetoInteracao.SendMessage("Interacao", SendMessageOptions.DontRequireReceiver);

        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking) playerRB.AddForce(new Vector2(0, jumpForce));

        if (isAttacking && isGrounded)
        {
            h = 0;
        }

        if (v < 0 && isGrounded)
        {
            crouching.enabled = true;
            standing.enabled = false;
        }
        else if (v >= 0 && isGrounded)
        {
            crouching.enabled = false;
            standing.enabled = true;
        }
        else if (v != 0 && !isGrounded)
        {
            crouching.enabled = false;
            standing.enabled = true;
        }


        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetInteger("idAnimation", idAnimation);
        playerAnimator.SetFloat("speedY", playerRB.velocity.y);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, whatIsGround);
        playerRB.velocity = new Vector2(h * playerSpeed, playerRB.velocity.y);
        Interagir();
    }

    void Flip() 
    {
        lookingLeft = !lookingLeft;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        dir.x = x;
    }

    void Attack(int atk) 
    {
        switch (atk) 
        {
            case 0:
                isAttacking = false;
                armas[2].SetActive(false);
                break;
            case 1:
                isAttacking = true;
                break;
        }
    }

    void ControleArma(int id)
    {
        foreach (GameObject o in armas) 
        {
            o.SetActive(false);
        }

        armas[id].SetActive(true);
    }

    void Interagir() 
    {
        RaycastHit2D hit = Physics2D.Raycast(hand.position, dir, 0.1f, interacao);
        if (hit)
        {
            objetoInteracao = hit.collider.gameObject;
            balaoAlerta.SetActive(true);
        }
        else
        {
            objetoInteracao = null;
            balaoAlerta.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "coletavel":
                collider.gameObject.SendMessage("Coletar", SendMessageOptions.DontRequireReceiver);
                break;
        }
    }

}

