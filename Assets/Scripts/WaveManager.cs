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

    private int waveAtual = 0;
    private int inimigosVivos = 0;

    private void Start()
    {
        StartCoroutine(IniciarProximaWave());
    }

    private IEnumerator IniciarProximaWave()
    {
        yield return new WaitForSeconds(tempoEntreWaves);
        hud.AtualizarWave(waveAtual);

        waveAtual++;

        int quantidadeInimigos =
            inimigosIniciais + waveAtual;

        Debug.Log("Wave " + waveAtual);

        hud.AtualizarWave(waveAtual);

        for (int i = 0; i < quantidadeInimigos; i++)
        {
            SpawnarInimigo();
        }

        hud.AtualizarInimigos(inimigosVivos);
    }

    private void SpawnarInimigo()
    {
        int indice =
            Random.Range(0, spawnPoints.Length);

        Transform spawnPoint =
            spawnPoints[indice];

        GameObject inimigo =
            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

        inimigosVivos++;

        hud.AtualizarInimigos(inimigosVivos);

        EnemyHealth enemyHealth =
            inimigo.GetComponent<EnemyHealth>();

        enemyHealth.OnEnemyDeath += InimigoMorreu;
    }

    private void InimigoMorreu()
    {
        inimigosVivos--;

        hud.AtualizarInimigos(inimigosVivos);

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