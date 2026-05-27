using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private float duracaoHurt = 0.4f;
    [SerializeField] private float duracaoMorte = 1.5f;
    [SerializeField] private HUDManager hud;
    private int vidaAtual;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PlayerMovement playerMovement;
    private PlayerMovement[] playerMovements;
    [SerializeField] private Collider2D col;

    private bool tomandoDano;
    private bool morto;

    public bool EstaMorto => morto;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (rigidBody == null)
            rigidBody = GetComponentInParent<Rigidbody2D>();

        if (playerMovement == null)
            playerMovement = GetComponentInParent<PlayerMovement>();

        if (playerMovement != null)
            playerMovements = new[] { playerMovement };
        else
            playerMovements = GetComponentsInParent<PlayerMovement>();

        if (col == null)
            col = GetComponentInParent<Collider2D>();
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

        if (vidaAtual < 0)
            vidaAtual = 0;

        hud.AtualizarVida(vidaAtual);

        Debug.Log("Player tomou dano. Vida atual: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
            return;
        }

        StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        tomandoDano = true;

        rigidBody.linearVelocity = Vector2.zero;

        if (playerMovements != null)
        {
            foreach (var movement in playerMovements)
            {
                if (movement != null)
                    movement.enabled = false;
            }
        }

        animator.Play("PlayerHurt", 0, 0f);

        yield return new WaitForSeconds(duracaoHurt);

        tomandoDano = false;

        if (!morto && playerMovements != null)
        {
            foreach (var movement in playerMovements)
            {
                if (movement != null)
                    movement.enabled = true;
            }
        }
    }

    private void Morrer()
    {
        morto = true;

        StopAllCoroutines();

        if (rigidBody != null)
            rigidBody.linearVelocity = Vector2.zero;

        if (playerMovements != null)
        {
            foreach (var movement in playerMovements)
            {
                if (movement != null)
                    movement.enabled = false;
            }
        }

        if (col != null)
            col.enabled = false;

        var knockbacks = GetComponentsInParent<PlayerKnockback>();
        if (knockbacks != null)
        {
            foreach (var knockback in knockbacks)
            {
                if (knockback != null)
                    knockback.enabled = false;
            }
        }

        GetComponent<PlayerAnimationController>().PlayDeath();
        if (hud != null)
            hud.MostrarGameOver();
    }
}