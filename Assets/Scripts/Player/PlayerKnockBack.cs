using System.Collections;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] private float tempoKnockback = 0.15f;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void AplicarKnockback(Transform origem, float forca)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackCoroutine(origem, forca));
    }

    private IEnumerator KnockbackCoroutine(Transform origem, float forca)
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        Vector2 direcao =
            (transform.position - origem.position).normalized;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(direcao * forca, ForceMode2D.Impulse);

        yield return new WaitForSeconds(tempoKnockback);

        rb.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}