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
    [SerializeField] private float dificuldade = 0.75f; // Fator de dificuldade que aumenta o número de inimigos a cada onda

    [Header("Referências")]
    [SerializeField] private List<GameObject> prefabInimigo = new List<GameObject>(); // Lista de prefabs dos inimigos para spawnar

    private int ordaAtual = 1; // Contador da onda atual
    private float tempodepoisdospawn; // Tempo acumulado desde o último spawn de inimigo
    private int inimigoVivo; // Contador de inimigos vivos no momento
    private int inimigosSaiuDoSpawn; // Contador de inimigos a serem spawnados na onda atual
    private bool estaSpawnando = false; // Indica se a onda atual está spawnando inimigos
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento estático chamado quando um inimigo é destruído

    private void Awake()
    {
        // Assina o evento de destruição de inimigo
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda
    }

    private void Update()
    {
        if (!estaSpawnando) return; // Se não está spawnando, não faz nada

        tempodepoisdospawn += Time.deltaTime; // Atualiza o tempo desde o último spawn

        // Verifica se é hora de spawnar um inimigo
        if (tempodepoisdospawn >= (1f / inimigosPorSeg) && inimigosSaiuDoSpawn > 0)
        {
            SpawnarInimigo(); // Chama o método para spawnar um inimigo
            inimigosSaiuDoSpawn--; // Decrementa o contador de inimigos a serem spawnados
            inimigoVivo++; // Incrementa o contador de inimigos vivos
            tempodepoisdospawn = 0f; // Reinicia o temporizador
        }

        // Inicia a próxima onda se todos os inimigos da onda atual foram derrotados
        if (inimigoVivo <= 0 && inimigosSaiuDoSpawn <= 0)
        {
            TerminarOrda(); // Chama o método para terminar a onda
        }
    }

    private void EnemyDestroyed()
    {
        // Reduz o contador de inimigos vivos quando um inimigo é destruído
        inimigoVivo--;
    }

    private void TerminarOrda()
    {
        estaSpawnando = false; // Para o spawn da onda atual
        tempodepoisdospawn = 0f; // Reinicia o temporizador
        ordaAtual++; // Incrementa a contagem de ondas
        StartCoroutine(StartWave()); // Inicia uma nova onda
        AdManager.instance.ShowNextAd(); // Exibe o próximo anúncio
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(tempoEntreOrdas); // Espera o tempo definido antes de iniciar a nova onda
        estaSpawnando = true; // Define que a nova onda está em processo de spawn
        inimigosSaiuDoSpawn = InimigoPorOrda(); // Calcula a quantidade de inimigos na nova onda
    }

    private int InimigoPorOrda()
    {
        // Calcula a quantidade de inimigos com base na dificuldade e na onda
        return Mathf.RoundToInt(inimigoBase * Mathf.Pow(ordaAtual, dificuldade));
    }

    private void SpawnarInimigo()
    {
        // Verifica se a lista de prefabs não está vazia
        if (prefabInimigo.Count == 0)
        {
            Debug.LogWarning("A lista de prefabs de inimigos está vazia!"); // Log de aviso
            return; // Sai do método se não houver prefabs
        }

        // Seleciona um prefab aleatório da lista
        GameObject prefabParaSpawnar = prefabInimigo[Random.Range(0, prefabInimigo.Count)];
        // Instancia o inimigo na posição de início definida pelo LevelManager
        _ = Instantiate(prefabParaSpawnar, LevelManager.main.startPoint.position, Quaternion.identity);
    }
}
