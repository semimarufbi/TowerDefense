using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe respons�vel pela gest�o do n�vel, incluindo caminho e moedas
public class LevelManager : MonoBehaviour
{

    public static LevelManager main; // Inst�ncia est�tica para acesso global
    public Transform[] path; // Array que armazena os pontos do caminho
    public Transform startPoint; // Ponto inicial do caminho

    public int currency; // Moeda do jogador

    private void Awake()
    {
        main = this; // Inicializa a inst�ncia est�tica
    }

    private void Start()
    {
        currency = 100; // Define a moeda inicial do jogador
    }

    // M�todo para aumentar a quantidade de moeda
    public void IncreaseCurerency(int amanunt)
    {
        currency += amanunt;
    }

    // M�todo para gastar moeda, retorna verdadeiro se a transa��o foi bem-sucedida
    public bool SpendCurency(int amanunt)
    {
        if (amanunt <= currency) // Verifica se h� moeda suficiente
        {
            currency -= amanunt; // Deduz a quantidade gasta
            return true; // Retorna verdadeiro se a transa��o foi bem-sucedida
        }
        else
        {
            return false; // Retorna falso se n�o h� moeda suficiente
        }
    }
}
