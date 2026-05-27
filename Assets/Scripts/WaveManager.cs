using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Config")]
    [SerializeField] private int inimigosIniciais = 3;
    [SerializeField] private float tempoEntreWaves = 3f;

    [Header("HUD")]
    [SerializeField] private HUDManager hud;
    [SerializeField] private float tempoEntreSpawns = 0.5f;

    private int waveAtual = 0;
    private int inimigosVivos = 0;

    private void Start()
    {
        StartCoroutine(IniciarProximaWave());
    }

    private IEnumerator IniciarProximaWave()
    {
        yield return new WaitForSeconds(tempoEntreWaves);

        waveAtual++;

        int quantidadeInimigos =
            inimigosIniciais + waveAtual;

        Debug.Log("Wave " + waveAtual);

        hud.AtualizarWave(waveAtual);
        hud.MostrarWavePopup(waveAtual);

        for (int i = 0; i < quantidadeInimigos; i++)
        {
            SpawnarInimigo();

            yield return new WaitForSeconds(tempoEntreSpawns);
        }
    }

    private void SpawnarInimigo()
    {
        int indice = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[indice];

        GameObject inimigo = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        int vidaExtra = waveAtual - 1;
        float velocidadeExtra = (waveAtual - 1) * 0.2f;
        int danoExtra = (waveAtual - 1) / 3;

        EnemyHealth enemyHealth = inimigo.GetComponent<EnemyHealth>();
        EnemyIA enemyIA = inimigo.GetComponent<EnemyIA>();

        if (enemyHealth != null)
        {
            enemyHealth.ConfigurarVida(vidaExtra);
            enemyHealth.OnEnemyDeath += InimigoMorreu;
        }

        if (enemyIA != null)
        {
            enemyIA.ConfigurarDificuldade(velocidadeExtra, danoExtra);
        }

        inimigosVivos++;

    }

    private void InimigoMorreu()
    {
        inimigosVivos--;


        Debug.Log(
            "Inimigos restantes: " +
            inimigosVivos
        );

        if (inimigosVivos <= 0)
        {
            StartCoroutine(IniciarProximaWave());
        }
    }
}