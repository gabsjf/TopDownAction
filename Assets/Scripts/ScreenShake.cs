using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Vector3 posicaoOriginal;
    private Coroutine shakeCoroutine;

    public void Shake(float duracao, float intensidade)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duracao, intensidade));
    }

    private IEnumerator ShakeRoutine(float duracao, float intensidade)
    {
        posicaoOriginal = transform.localPosition;

        float tempo = 0f;

        while (tempo < duracao)
        {
            float x = Random.Range(-1f, 1f) * intensidade;
            float y = Random.Range(-1f, 1f) * intensidade;

            transform.localPosition = posicaoOriginal + new Vector3(x, y, 0f);

            tempo += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = posicaoOriginal;
        shakeCoroutine = null;
    }
}