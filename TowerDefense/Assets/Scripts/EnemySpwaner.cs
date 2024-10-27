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
    [SerializeField] private float dificuldade = 0.75f; // Dificuldade que aumenta o n�mero de inimigos
    [SerializeField] private int maxOrdas = 10; // N�mero m�ximo de ordas a serem spawnadas

    [Header("Refer�ncias")]
    [SerializeField] private GameObject[] prefabInimigo; // Prefabs dos inimigos

    private int ordaAtual = 1; // Contador de ordas atual
    private float tempodepoisdospawn; // Tempo ap�s o �ltimo spawn
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

        // Verifica se o n�mero m�ximo de ordas foi atingido
        if (ordaAtual <= maxOrdas)
        {
            StartCoroutine(StartWave());
        }
        else
        {
            Debug.Log("N�mero m�ximo de ordas atingido. N�o haver� mais inimigos.");
            // Aqui voc� pode adicionar a l�gica de final de jogo ou transi��o para a pr�xima fase
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
