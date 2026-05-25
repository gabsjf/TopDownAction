using UnityEngine;
using System.Collections;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private float distanciaMinima = 1.2f;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int danoAtaque = 1;
    [SerializeField] private float cooldownAtaque = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip orcHitSfx;

    private Transform alvo;
    private bool atacando;
    private Animator animator;
    private bool pausada;
    private bool morto;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            alvo = player.transform;

        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (morto || pausada || alvo == null)
        {
            rigidBody.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 posicaoAlvo = alvo.position;
        Vector2 posicaoAtual = transform.position;

        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);

        if (distancia > distanciaMinima)
        {
            Vector2 direcao = (posicaoAlvo - posicaoAtual).normalized;

            rigidBody.linearVelocity = velocidade * direcao;

            animator.Play("EnemyWalk");

            if (rigidBody.linearVelocity.x > 0)
                spriteRenderer.flipX = false;
            else if (rigidBody.linearVelocity.x < 0)
                spriteRenderer.flipX = true;
        }
        else
        {
            rigidBody.linearVelocity = Vector2.zero;

            if (!atacando)
                StartCoroutine(Atacar());
        }
    }

    public void PausarIA(bool devePausar)
    {
        pausada = devePausar;

        if (pausada && rigidBody != null)
            rigidBody.linearVelocity = Vector2.zero;
    }

    public void MarcarComoMorto()
    {
        morto = true;

        if (rigidBody != null)
            rigidBody.linearVelocity = Vector2.zero;

        enabled = false;
    }

    private IEnumerator Atacar()
    {
        atacando = true;

        rigidBody.linearVelocity = Vector2.zero;

        int ataque = Random.Range(0, 2);

        if (ataque == 0)
            animator.Play("EnemyAtaq1");
        else
            animator.Play("EnemyAtaq2");

        yield return new WaitForSeconds(cooldownAtaque);

        atacando = false;
    }

    public void AplicarDanoDoAtaque()
    {
        bool acertou = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            PlayerHealth playerHealth =
                hit.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TomarDano(danoAtaque);
                acertou = true;
            }
        }

        if (acertou && audioSource != null && orcHitSfx != null)
            audioSource.PlayOneShot(orcHitSfx);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}