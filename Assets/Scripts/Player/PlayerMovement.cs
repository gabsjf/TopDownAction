using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float vel = 5f;

    [Header("Ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int danoBasico = 1;
    [SerializeField] private int danoForte = 2;
    [SerializeField] private Vector2 tamanhoAtaqueForte = new Vector2(1.5f, 2f);
    public float knockbackForce = 70f;
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] private float shakeDuracao = 0.08f;
    [SerializeField] private float shakeIntensidade = 0.08f;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playerHitSfx;

    [Header("Combo")]
    [SerializeField] private float janelaCombo = 0.6f;
    [SerializeField] private int danoComboFinal = 3;
    [SerializeField] private GameObject hitParticlePrefab;
    private Rigidbody2D rb;
    private Vector2 inputMovimento;
    private PlayerAnimationController playerAnim;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;

    private bool atacando;
    private Vector2 direcaoOlhando = Vector2.down;
    private int danoAtual;
    private bool ataqueForteAtual;
    private int comboAtual = 0;
    private float tempoUltimoAtaque;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.EstaMorto)
            return;

        LerInput();
        AtqBasico();
        AtqForte();
    }

    private void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.EstaMorto)
            return;

        Move();
        Animar();
    }

    private void LerInput()
    {
        inputMovimento = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            inputMovimento.y += 1;

        if (Keyboard.current.sKey.isPressed)
            inputMovimento.y -= 1;

        if (Keyboard.current.aKey.isPressed)
        {
            inputMovimento.x -= 1;
            spriteRenderer.flipX = true;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            inputMovimento.x += 1;
            spriteRenderer.flipX = false;
        }

        inputMovimento = inputMovimento.normalized;

        if (inputMovimento != Vector2.zero)
            direcaoOlhando = inputMovimento;
    }

    private void AtqBasico()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !atacando)
        {
            if (Time.time - tempoUltimoAtaque > janelaCombo)
            {
                comboAtual = 0;
            }

            comboAtual++;
            tempoUltimoAtaque = Time.time;

            if (comboAtual == 1)
            {
                StartCoroutine(Atacar("PlayerAtaq", 0.3f, danoBasico, false));
            }
            else if (comboAtual == 2)
            {
                StartCoroutine(Atacar("PlayerAtaq", 0.3f, danoBasico, false));
            }
            else
            {
                StartCoroutine(Atacar("PlayerAtaq2", 0.45f, danoComboFinal, true));
                comboAtual = 0;
            }
        }
    }
 
    private void AtqForte()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !atacando)
        {
            comboAtual = 0;
            StartCoroutine(Atacar("PlayerAtaq2", 0.7f, danoForte, true));
        }
    }

    private void Animar()
    {
        if (atacando) return;

        if (inputMovimento != Vector2.zero)
            playerAnim.PlayAnimation("PlayerWalk");
        else
            playerAnim.PlayAnimation("PlayerIdle");
    }

    private System.Collections.IEnumerator Atacar(string animacao, float duracao, int dano, bool ataqueForte)
    {
        atacando = true;
        danoAtual = dano;
        ataqueForteAtual = ataqueForte;

        playerAnim.PlayAnimation(animacao);

        yield return new WaitForSeconds(duracao);

        atacando = false;
    }

    public void AplicarDanoDoAtaque()
    {
        if (ataqueForteAtual)
            CausarDanoForte(danoAtual);
        else
            CausarDano(danoAtual);
    }

    private void CausarDano(int dano)
    {
        bool acertou = false;

        Collider2D[] inimigos = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D inimigo in inimigos)
        {
            Vector2 direcaoParaInimigo =
                ((Vector2)inimigo.transform.position - (Vector2)transform.position).normalized;

            float alinhamento = Vector2.Dot(direcaoOlhando.normalized, direcaoParaInimigo);

            if (alinhamento > 0f)
            {
                EnemyHealth enemyHealth = inimigo.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TomaDano(dano);
                    if (hitParticlePrefab != null)
                    {
                        GameObject particula = Instantiate(
                            hitParticlePrefab,
                            inimigo.transform.position,
                            Quaternion.identity
                        );

                        Destroy(particula, 1f);
                    }
                    acertou = true;

                    EnemyKnockback knockback = inimigo.GetComponent<EnemyKnockback>();

                    if (knockback != null)
                    {
                        knockback.AplicarKnockback(transform, 8f);
                    }
                    if (screenShake != null)
                    {
                        screenShake.Shake(shakeDuracao, shakeIntensidade);
                    }
                }
            }
        }

        if (acertou && audioSource != null && playerHitSfx != null)
            audioSource.PlayOneShot(playerHitSfx);
    }

    private void CausarDanoForte(int dano)
    {
        bool acertou = false;

        Vector2 centroAtaque =
            (Vector2)transform.position + direcaoOlhando.normalized * 0.8f;

        Collider2D[] inimigos = Physics2D.OverlapBoxAll(
            centroAtaque,
            tamanhoAtaqueForte,
            0f,
            enemyLayer
        );

        foreach (Collider2D inimigo in inimigos)
        {
            EnemyHealth enemyHealth = inimigo.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TomaDano(dano);
                if (hitParticlePrefab != null)
                {
                    GameObject particula = Instantiate(
                        hitParticlePrefab,
                        inimigo.transform.position,
                        Quaternion.identity
                    );

                    Destroy(particula, 1f);
                }
                acertou = true;
                EnemyKnockback knockback = inimigo.GetComponent<EnemyKnockback>();

                if (knockback != null)
                {
                    knockback.AplicarKnockback(transform, 8f);
                }
                if (screenShake != null)
                {
                    screenShake.Shake(shakeDuracao, shakeIntensidade);
                }
            }
        }

        if (acertou && audioSource != null && playerHitSfx != null)
            audioSource.PlayOneShot(playerHitSfx);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.yellow;
        Vector2 centroAtaque =
            (Vector2)transform.position + direcaoOlhando.normalized * 0.8f;

        Gizmos.DrawWireCube(centroAtaque, tamanhoAtaqueForte);
    }

    private void Move()
    {
        rb.linearVelocity = inputMovimento * vel;
    }
}