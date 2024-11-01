using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Instância única do BuildManager (Singleton)
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers; // Array de torres disponíveis para construção

    [Header("Attributes")]
    private int selectedTower = 0; // Índice da torre selecionada atualmente

    // Método chamado na inicialização do objeto
    private void Awake()
    {
        Instance = this; // Define a instância do BuildManager
    }

    // Método para obter a torre atualmente selecionada
    public Tower GetSelectedTower()
    {
        return towers[selectedTower]; // Retorna a torre com base no índice selecionado
    }

    // Método para definir a torre selecionada
    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower; // Atualiza o índice da torre selecionada
    }
}
