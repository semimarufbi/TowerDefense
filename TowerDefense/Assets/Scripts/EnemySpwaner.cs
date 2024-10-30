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
     // Número máximo de ondas que serão spawnadas

    [Header("Referências")]
    [SerializeField] private GameObject[] prefabInimigo; // Array de prefabs dos inimigos para spawnar

    private int ordaAtual = 1; // Contador da onda atual
    private float tempodepoisdospawn; // Tempo acumulado desde o último spawn de inimigo
    private int inimigoVivo; // Contador de inimigos vivos no momento
    private int inimigosSaiuDoSpawn; // Contador de inimigos a serem spawnados na onda atual
    private bool estaSpawnando = false; // Indica se a onda atual está spawnando inimigos
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento estático chamado quando um inimigo é destruído

    private void Awake()
    {
        // Adiciona o método EnemyDestroyed ao evento onEnemyDestroy, para reduzir o contador de inimigos vivos quando um inimigo é destruído
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        // Inicia a primeira onda assim que o jogo começa
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // Se não estiver spawnando inimigos, não realiza nenhuma ação
        if (!estaSpawnando) return;

        // Atualiza o tempo desde o último spawn de inimigo
        tempodepoisdospawn += Time.deltaTime;

        // Verifica se é hora de spawnar um inimigo
        if (tempodepoisdospawn >= (1f / inimigosPorSeg) && inimigosSaiuDoSpawn > 0)
        {
            SpawnarInimigo(); // Cria um novo inimigo
            inimigosSaiuDoSpawn--; // Diminui o contador de inimigos a serem spawnados
            inimigoVivo++; // Incrementa o contador de inimigos vivos
            tempodepoisdospawn = 0f; // Reseta o tempo desde o último spawn
        }

        // Verifica se todos os inimigos da onda foram derrotados
        if (inimigoVivo == 0 && inimigosSaiuDoSpawn == 0)
        {
            TerminarOrda(); // Finaliza a onda atual e prepara a próxima
        }
    }

    private void EnemyDestroyed()
    {
        // Decrementa o contador de inimigos vivos ao ser chamado pelo evento de destruição do inimigo
        inimigoVivo--;
    }

    private void TerminarOrda()
    {
        // Para o spawn de inimigos e prepara a próxima onda
        estaSpawnando = false;
        tempodepoisdospawn = 0f; // Reseta o tempo desde o último spawn
        ordaAtual++; // Incrementa o contador de ondas
        StartCoroutine(StartWave()); // Inicia a próxima onda
        
       
    }

    private IEnumerator StartWave()
    {
        // Espera pelo intervalo entre ondas e então inicia a geração de inimigos
        yield return new WaitForSeconds(tempoEntreOrdas);
        estaSpawnando = true; // Define que a onda está em processo de spawn
        inimigosSaiuDoSpawn = InimigoPorOrda(); // Define o número de inimigos a serem spawnados na onda atual
    }

    private int InimigoPorOrda()
    {
        // Calcula a quantidade de inimigos da onda com base na dificuldade e na onda atual
        return Mathf.RoundToInt(inimigoBase * Mathf.Pow(ordaAtual, dificuldade));
    }

    private void SpawnarInimigo()
    {
        // Escolhe um prefab de inimigo aleatório e o spawn no ponto inicial do LevelManager
        GameObject prefabParaSpawnar = prefabInimigo[Random.Range(0, prefabInimigo.Length)];
        Instantiate(prefabParaSpawnar, LevelManager.main.startPoint.position, Quaternion.identity);
    }
}
