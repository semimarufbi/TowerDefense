using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int inimigoBase = 8; // Base de inimigos por onda
    [SerializeField] private float inimigosPorSeg = 0.5f; // Quantidade de inimigos por segundo
    [SerializeField] private float tempoEntreOrdas = 5f; // Tempo entre ordas
    [SerializeField] private float dificuldade = 0.75f; // Dificuldade que aumenta o número de inimigos
    [SerializeField] private int maxOrdas = 10; // Número máximo de ordas a serem spawnadas

    [Header("Referências")]
    [SerializeField] private GameObject[] prefabInimigo; // Prefabs dos inimigos

    private int ordaAtual = 1; // Contador de ordas atual
    private float tempodepoisdospawn; // Tempo após o último spawn
    private int inimigoVivo; // Contador de inimigos vivos
    private int inimigosSaiuDoSpawn; // Contador de inimigos que foram spawnados
    private bool estaSpawnando = false; // Estado do spawn
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!estaSpawnando) return;

        tempodepoisdospawn += Time.deltaTime;
        if (tempodepoisdospawn >= (1f / inimigosPorSeg) && inimigosSaiuDoSpawn > 0)
        {
            SpawnarInimigo();
            inimigosSaiuDoSpawn--;
            inimigoVivo++;
            tempodepoisdospawn = 0f;
        }

        if (inimigoVivo == 0 && inimigosSaiuDoSpawn == 0)
        {
            TerminarOrda();
        }
    }

    private void EnemyDestroyed()
    {
        inimigoVivo--;
    }

    private void TerminarOrda()
    {
        estaSpawnando = false;
        tempodepoisdospawn = 0f;
        ordaAtual++;

        // Verifica se o número máximo de ordas foi atingido
        if (ordaAtual <= maxOrdas)
        {
            StartCoroutine(StartWave());
        }
        else
        {
            Debug.Log("Número máximo de ordas atingido. Não haverá mais inimigos.");
            // Aqui você pode adicionar a lógica de final de jogo ou transição para a próxima fase
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(tempoEntreOrdas);
        estaSpawnando = true;
        inimigosSaiuDoSpawn = InimigoPorOrda();
    }

    private int InimigoPorOrda()
    {
        return Mathf.RoundToInt(inimigoBase * Mathf.Pow(ordaAtual, dificuldade));
    }

    private void SpawnarInimigo()
    {
        GameObject prefabParaSpawnar = prefabInimigo[Random.Range(0, prefabInimigo.Length)];
        Instantiate(prefabParaSpawnar, LevelManager.main.startPoint.position, Quaternion.identity);
    }
}
