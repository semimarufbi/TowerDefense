using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe responsável pela gestão do nível, incluindo caminho e moedas
public class LevelManager : MonoBehaviour
{

    public static LevelManager main; // Instância estática para acesso global
    public Transform[] path; // Array que armazena os pontos do caminho
    public Transform startPoint; // Ponto inicial do caminho

    public int currency; // Moeda do jogador

    private void Awake()
    {
        main = this; // Inicializa a instância estática
    }

    private void Start()
    {
        currency = 100; // Define a moeda inicial do jogador
    }

    // Método para aumentar a quantidade de moeda
    public void IncreaseCurerency(int amanunt)
    {
        currency += amanunt;
    }

    // Método para gastar moeda, retorna verdadeiro se a transação foi bem-sucedida
    public bool SpendCurency(int amanunt)
    {
        if (amanunt <= currency) // Verifica se há moeda suficiente
        {
            currency -= amanunt; // Deduz a quantidade gasta
            return true; // Retorna verdadeiro se a transação foi bem-sucedida
        }
        else
        {
            return false; // Retorna falso se não há moeda suficiente
        }
    }
}
