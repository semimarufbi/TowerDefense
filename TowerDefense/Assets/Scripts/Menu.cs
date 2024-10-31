using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private CanvasGroup menuCanvasGroup; // CanvasGroup permite ocultar o menu sem desativá-lo

    private bool isMenuVisible = false; // Controle de visibilidade do menu

    private void Start()
    {
        if (menuCanvasGroup != null)
        {
            // Inicializa o menu como oculto
            menuCanvasGroup.alpha = 0f;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        // Verifica se a tecla "E" foi pressionada
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleMenu(); // Alterna a visibilidade do menu
        }
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    private void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible; // Alterna o estado de visibilidade
        menuCanvasGroup.alpha = isMenuVisible ? 1f : 0f; // Define a opacidade do menu
        menuCanvasGroup.interactable = isMenuVisible; // Habilita/desabilita a interação
        menuCanvasGroup.blocksRaycasts = isMenuVisible; // Define se o menu bloqueia interações com outros elementos
    }
}
