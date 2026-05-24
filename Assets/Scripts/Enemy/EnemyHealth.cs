using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 5;

    private int vidaAtual;
    private Animator animator;
    private bool tomandoDano;
    private bool morto;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void TomaDano(int dano)
    {
        if (morto)
            return;

        if (tomandoDano)
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

        animator.Play("EnemyHurt");

        yield return new WaitForSeconds(0.4f);

        tomandoDano = false;

        if (!morto)
        {
            animator.Play("EnemyIdle");
        }
    }

    private void Morrer()
    {
        morto = true;

        tomandoDano = false;
        StopAllCoroutines();

        Debug.Log("Tocando animação de morte");

        animator.Play("EnemyDead", 0, 0f);

        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 1.5f);
    }
}