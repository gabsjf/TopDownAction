using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private float duracaoHurt = 0.4f;
    [SerializeField] private float duracaoMorte = 1.2f;
    public System.Action OnEnemyDeath;
    private int vidaAtual;
    private Animator animator;
    private EnemyIA enemyIA;
    private Collider2D col;

    private bool morto;
    private bool tomandoDano;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyIA = GetComponent<EnemyIA>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TomaDano(int dano)
    {
        if (morto || tomandoDano)
            return;

        vidaAtual -= dano;

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

        if (enemyIA != null)
            enemyIA.PausarIA(true);

        animator.Play("EnemyHurt", 0, 0f);

        yield return new WaitForSeconds(duracaoHurt);

        tomandoDano = false;

        if (enemyIA != null)
            enemyIA.PausarIA(false);
    }

    public void ConfigurarVida(int vidaExtra)
    {
        vidaMaxima += vidaExtra;
        vidaAtual = vidaMaxima;
    }

    private void Morrer()
    {
        morto = true;
        tomandoDano = false;

        StopAllCoroutines();

        if (enemyIA != null)
            enemyIA.MarcarComoMorto();

        if (col != null)
            col.enabled = false;

        animator.Play("EnemyDead", 0, 0f);

        OnEnemyDeath?.Invoke();
        Destroy(gameObject, duracaoMorte);
    }
}