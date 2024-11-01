using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Inst�ncia �nica do BuildManager (Singleton)
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers; // Array de torres dispon�veis para constru��o

    [Header("Attributes")]
    private int selectedTower = 0; // �ndice da torre selecionada atualmente

    // M�todo chamado na inicializa��o do objeto
    private void Awake()
    {
        Instance = this; // Define a inst�ncia do BuildManager
    }

    // M�todo para obter a torre atualmente selecionada
    public Tower GetSelectedTower()
    {
        return towers[selectedTower]; // Retorna a torre com base no �ndice selecionado
    }

    // M�todo para definir a torre selecionada
    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower; // Atualiza o �ndice da torre selecionada
    }
}
