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
    }

    private void AtqBasico()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !atacando)
        {
            StartCoroutine(Atacar("PlayerAtaq", 0.4f));
        }
    }

    private void AtqForte()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !atacando)
        {
            StartCoroutine(Atacar("PlayerAtaq2", 0.6f));
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

    private System.Collections.IEnumerator Atacar(string animacao, float duracao)
    {
        atacando = true;
        playerAnim.PlayAnimation(animacao);

        yield return new WaitForSeconds(duracao);

        atacando = false;
    }
    private void Move()
    {
        rb.linearVelocity = inputMovimento * vel;
    }
}
