using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform alvo;
    [SerializeField] private float suavidade = 5f;

    private void LateUpdate()
    {
        Vector3 posicaoDesejada = new Vector3(
            alvo.position.x,
            alvo.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavidade * Time.deltaTime
        );
    }
}