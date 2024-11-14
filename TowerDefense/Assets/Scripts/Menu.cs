using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI; // Refer�ncia ao componente TextMeshPro que exibe a quantidade de moeda
    
    
   

    
    // M�todo chamado para atualizar o UI (User Interface)
    private void OnGUI()
    {
        // Atualiza o texto da moeda no menu com a quantidade atual
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    
}
