using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;
    private bool isGameOver = false; // Para evitar chamadas repetidas do Game Over

    public GameObject gameOverPanel; // Painel de Game Over (defina no Inspector)

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Método para adicionar 100 moedas como recompensa
    public void RewardCurrency()
    {
        int reward = Random.Range(100, 1000);
        IncreaseCurrency(reward);
        Debug.Log($"Você ganhou {reward} moedas!");
    }

    public void GameOver()
    {
        if (isGameOver) return; // Evita que o Game Over seja chamado várias vezes

        isGameOver = true; // Marca que o jogo terminou
        Debug.Log("Game Over! Um inimigo alcançou o ponto final.");

        // Exibe o painel de Game Over
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        if (!AdManager.instance.isGamePausedByAd)
        {
            Time.timeScale = 0; // Apenas pausa o jogo se não estiver pausado por um anúncio
        }






    }
    public void Reiniciar()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        isGameOver=false;

    }
}
