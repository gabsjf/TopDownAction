using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float vel = 5f;
    private Rigidbody2D rb;
    private Vector2 inputMovimento;
    private PlayerAnimationController playerAnim;
    private SpriteRenderer spriteRenderer;
    private bool atacando;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int danoBasico = 1;
    [SerializeField] private int danoForte = 2;
    private Vector2 direcaoOlhando = Vector2.down;
    [SerializeField] private Vector2 tamanhoAtaqueForte = new Vector2(1.5f, 2f);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        LerInput();
        AtqBasico();
        AtqForte();
    }

    private void FixedUpdate()
    {
        Move();
        Animar();
        
    }
    private void LerInput()
    {
        inputMovimento = Vector2.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            inputMovimento.y += 1;
            
        }
        
        if (Keyboard.current.sKey.isPressed)
        {
            inputMovimento.y -= 1;
           
        }
        
        if (Keyboard.current.aKey.isPressed)
        {
            inputMovimento.x -= 1;
            spriteRenderer.flipX = true;

        }
        else
        
        if (Keyboard.current.dKey.isPressed)
        {
            inputMovimento.x += 1;
                spriteRenderer.flipX = false;

            }

        inputMovimento = inputMovimento.normalized;
        if(inputMovimento != Vector2.zero)
        {
            direcaoOlhando = inputMovimento;

        }
    }

    private void AtqBasico()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !atacando)
        {
            StartCoroutine(Atacar("PlayerAtaq", 0.4f,danoBasico));
        }
    }

    private void AtqForte()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !atacando)
        {
            StartCoroutine(Atacar("PlayerAtaq2", 0.6f,danoForte));
        }
    }

    private void Animar()
    {
        if (atacando) return;

        if (inputMovimento != Vector2.zero)
        {
            playerAnim.PlayAnimation("PlayerWalk");
        }
        else
        {
            playerAnim.PlayAnimation("PlayerIdle");
        }
    }

    private System.Collections.IEnumerator Atacar(string animacao, float duracao, int dano)
    {
        atacando = true;
        playerAnim.PlayAnimation(animacao);
        if (animacao == "PlayerAtaq")
        {
            CausarDano(dano);
        }
        else if (animacao == "PlayerAtaq2")
        {
            CausarDanoForte(dano);
        }
        yield return new WaitForSeconds(duracao);

        atacando = false;
    }

    private void CausarDano(int dano)
    {
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
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
                }
            }
        }
    }

    private void CausarDanoForte(int dano)
    {
        Vector2 centroAtaque = (Vector2)transform.position + direcaoOlhando.normalized * 0.8f;

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
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    private void Move()
    {
        rb.linearVelocity = inputMovimento * vel;
    }
}
