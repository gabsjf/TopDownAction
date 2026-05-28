using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text VidaTexto;
    [SerializeField] private TMP_Text WaveTexto;
    [SerializeField] private GameObject GameOverTexto;
    [SerializeField] private TMP_Text WavePopupTexto;
    [SerializeField] private Slider BarraVida;


    private void Start()
    {
        GameOverTexto.SetActive(false);
        WavePopupTexto.gameObject.SetActive(false);
    }

    public void AtualizarVida(int vida)
    {
        VidaTexto.text = "Vida: " + vida;
        BarraVida.value = vida;
    }
    public void ConfigurarVidaMaxima(int vidaMaxima)
    {
        BarraVida.maxValue = vidaMaxima;
        BarraVida.value = vidaMaxima;
    }

    public void AtualizarWave(int wave)
    {
        WaveTexto.text = "Wave: " + wave;
    }

    public void MostrarGameOver()
    {
        GameOverTexto.SetActive(true);
    }

    public void MostrarWavePopup(int wave)
    {
        StartCoroutine(MostrarWavePopupRoutine(wave));
    }

    private IEnumerator MostrarWavePopupRoutine(int wave)
    {
        WavePopupTexto.gameObject.SetActive(true);
        WavePopupTexto.text = "WAVE " + wave;

        yield return new WaitForSeconds(1.2f);

        WavePopupTexto.gameObject.SetActive(false);
    }
}