using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float tempoKnockback = 0.15f;

    private Rigidbody2D rb;
    private EnemyIA enemyIA;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyIA = GetComponent<EnemyIA>();
    }

    public void AplicarKnockback(Transform origem, float forca)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackCoroutine(origem, forca));
    }

    private IEnumerator KnockbackCoroutine(Transform origem, float forca)
    {
        if (enemyIA != null)
            enemyIA.PausarIA(true);

        Vector2 direcao =
            (transform.position - origem.position).normalized;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(direcao * forca, ForceMode2D.Impulse);

        yield return new WaitForSeconds(tempoKnockback);

        rb.linearVelocity = Vector2.zero;

        if (enemyIA != null)
            enemyIA.PausarIA(false);
    }
}