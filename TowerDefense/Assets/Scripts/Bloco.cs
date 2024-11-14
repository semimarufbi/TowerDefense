using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloco : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Referência ao componente SpriteRenderer para alterar a cor do bloco
    [SerializeField] private Color hoverColor; // Cor que o bloco deve mudar quando o mouse passa sobre ele

    [Header("Attributes")]
    private GameObject tower; // Referência para a torre que pode ser construída neste bloco
    private Color startColor; // Armazena a cor original do bloco para restaurar quando o mouse sai

    private void Start()
    {
        // Salva a cor original do bloco ao iniciar o jogo
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        // Muda a cor do bloco para 'hoverColor' quando o mouse entra na área do bloco
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        // Restaura a cor original do bloco quando o mouse sai da área do bloco
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // Verifica se já existe uma torre neste bloco
        if (tower != null) return;

        // Obtém a torre selecionada através do BuildManager
        Tower towerToBuild = BuildManager.Instance.GetSelectedTower();

        // Verifica se há moeda suficiente para construir a torre
        if (towerToBuild.coast > LevelManager.main.currency)
        {
            return; // Não constrói a torre se não houver moedas suficientes
        }

        // Deduz o custo da construção da torre da moeda disponível
        LevelManager.main.SpendCurrency(towerToBuild.coast);

        // Instancia a torre na posição do bloco, atribuindo-a ao campo 'tower'
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
