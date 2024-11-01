using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI; // Referência ao componente TextMeshPro que exibe a quantidade de moeda
    [SerializeField] private CanvasGroup menuCanvasGroup; // Permite ocultar o menu sem desativá-lo

    private bool isMenuVisible = false; // Controle de visibilidade do menu

    // Método chamado na inicialização do objeto
    private void Start()
    {
        if (menuCanvasGroup != null)
        {
            // Inicializa o menu como oculto
            menuCanvasGroup.alpha = 0f; // Define a opacidade como 0
            menuCanvasGroup.interactable = false; // Desabilita a interação com o menu
            menuCanvasGroup.blocksRaycasts = false; // Impede que o menu bloqueie interações com outros elementos
        }
    }

    // Método chamado a cada frame
    private void Update()
    {
        // Verifica se a tecla "E" foi pressionada
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleMenu(); // Alterna a visibilidade do menu
        }
    }

    // Método chamado para atualizar o UI (User Interface)
    private void OnGUI()
    {
        // Atualiza o texto da moeda no menu com a quantidade atual
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    // Método para alternar a visibilidade do menu
    private void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible; // Alterna o estado de visibilidade
        menuCanvasGroup.alpha = isMenuVisible ? 1f : 0f; // Define a opacidade do menu: 1 se visível, 0 se oculto
        menuCanvasGroup.interactable = isMenuVisible; // Habilita/desabilita a interação com o menu
        menuCanvasGroup.blocksRaycasts = isMenuVisible; // Define se o menu bloqueia interações com outros elementos
    }
}
