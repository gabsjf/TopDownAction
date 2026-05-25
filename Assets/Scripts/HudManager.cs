using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text VidaTexto;
    [SerializeField] private TMP_Text WaveTexto;
    [SerializeField] private TMP_Text InimigoTexto;
    [SerializeField] private GameObject GameOverTexto;

    private void Start()
    {
        GameOverTexto.SetActive(false);
    }

    public void AtualizarVida(int vida)
    {
        VidaTexto.text = "Vida: " + vida;
    }

    public void AtualizarWave(int wave)
    {
        WaveTexto.text = "Wave: " + wave;
    }

    public void AtualizarInimigos(int quantidade)
    {
        InimigoTexto.text =
            "Inimigos: " + quantidade;
    }

    public void MostrarGameOver()
    {
        GameOverTexto.SetActive(true);
    }
}