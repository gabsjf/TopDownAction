using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private float duracaoHurt = 0.4f;
    [SerializeField] private float duracaoMorte = 1.5f;
    [SerializeField] private HUDManager hud;
    private int vidaAtual;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private PlayerMovement playerMovement;
    private Collider2D col;

    private bool tomandoDano;
    private bool morto;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
        hud.AtualizarVida(vidaAtual); 
    }

    public void TomarDano(int dano)
    {
        if (morto || tomandoDano)
            return;

        vidaAtual -= dano;

        Debug.Log("Player tomou dano. Vida atual: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
            return;
        }

        StartCoroutine(HurtRoutine());
        hud.AtualizarVida(vidaAtual);
    }

    private IEnumerator HurtRoutine()
    {
        tomandoDano = true;

        rigidBody.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = false;

        animator.Play("PlayerHurt", 0, 0f);

        yield return new WaitForSeconds(duracaoHurt);

        tomandoDano = false;

        if (!morto)
        {
            if (playerMovement != null)
                playerMovement.enabled = true;
        }
    }

    private void Morrer()
    {
        morto = true;

        StopAllCoroutines();

        rigidBody.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (col != null)
            col.enabled = false;

        animator.Play("PlayerDead", 0, 0f);

        Destroy(gameObject, duracaoMorte);
        hud.MostrarGameOver();
    }
}