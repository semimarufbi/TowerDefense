using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int inimigoBase = 8; // Quantidade base de inimigos por onda
    [SerializeField] private float inimigosPorSeg = 0.5f; // Quantidade de inimigos gerados por segundo
    [SerializeField] private float tempoEntreOrdas = 5f; // Intervalo de tempo entre cada onda
    [SerializeField] private float dificuldade = 0.75f; // Fator de dificuldade que aumenta o n�mero de inimigos a cada onda

    [Header("Refer�ncias")]
    [SerializeField] private List<GameObject> prefabInimigo = new List<GameObject>(); // Lista de prefabs dos inimigos para spawnar

    private int ordaAtual = 1; // Contador da onda atual
    private float tempodepoisdospawn; // Tempo acumulado desde o �ltimo spawn de inimigo
    private int inimigoVivo; // Contador de inimigos vivos no momento
    private int inimigosSaiuDoSpawn; // Contador de inimigos a serem spawnados na onda atual
    private bool estaSpawnando = false; // Indica se a onda atual est� spawnando inimigos
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento est�tico chamado quando um inimigo � destru�do

    private void Awake()
    {
        // Assina o evento de destrui��o de inimigo
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda
    }

    private void Update()
    {
        if (!estaSpawnando) return; // Se n�o est� spawnando, n�o faz nada

        tempodepoisdospawn += Time.deltaTime; // Atualiza o tempo desde o �ltimo spawn

        // Verifica se � hora de spawnar um inimigo
        if (tempodepoisdospawn >= (1f / inimigosPorSeg) && inimigosSaiuDoSpawn > 0)
        {
            SpawnarInimigo(); // Chama o m�todo para spawnar um inimigo
            inimigosSaiuDoSpawn--; // Decrementa o contador de inimigos a serem spawnados
            inimigoVivo++; // Incrementa o contador de inimigos vivos
            tempodepoisdospawn = 0f; // Reinicia o temporizador
        }

        // Inicia a pr�xima onda se todos os inimigos da onda atual foram derrotados
        if (inimigoVivo <= 0 && inimigosSaiuDoSpawn <= 0)
        {
            TerminarOrda(); // Chama o m�todo para terminar a onda
        }
    }

    private void EnemyDestroyed()
    {
        // Reduz o contador de inimigos vivos quando um inimigo � destru�do
        inimigoVivo--;
    }

    private void TerminarOrda()
    {
        estaSpawnando = false; // Para o spawn da onda atual
        tempodepoisdospawn = 0f; // Reinicia o temporizador
        ordaAtual++; // Incrementa a contagem de ondas
        StartCoroutine(StartWave()); // Inicia uma nova onda
        AdManager.instance.ShowNextAd(); // Exibe o pr�ximo an�ncio
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(tempoEntreOrdas); // Espera o tempo definido antes de iniciar a nova onda
        estaSpawnando = true; // Define que a nova onda est� em processo de spawn
        inimigosSaiuDoSpawn = InimigoPorOrda(); // Calcula a quantidade de inimigos na nova onda
    }

    private int InimigoPorOrda()
    {
        // Calcula a quantidade de inimigos com base na dificuldade e na onda
        return Mathf.RoundToInt(inimigoBase * Mathf.Pow(ordaAtual, dificuldade));
    }

    private void SpawnarInimigo()
    {
        // Verifica se a lista de prefabs n�o est� vazia
        if (prefabInimigo.Count == 0)
        {
            Debug.LogWarning("A lista de prefabs de inimigos est� vazia!"); // Log de aviso
            return; // Sai do m�todo se n�o houver prefabs
        }

        // Seleciona um prefab aleat�rio da lista
        GameObject prefabParaSpawnar = prefabInimigo[Random.Range(0, prefabInimigo.Count)];
        // Instancia o inimigo na posi��o de in�cio definida pelo LevelManager
        _ = Instantiate(prefabParaSpawnar, LevelManager.main.startPoint.position, Quaternion.identity);
    }
}
