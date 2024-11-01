using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloco : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Refer�ncia ao componente SpriteRenderer para alterar a cor do bloco
    [SerializeField] private Color hoverColor; // Cor que o bloco deve mudar quando o mouse passa sobre ele

    [Header("Attributes")]
    private GameObject tower; // Refer�ncia para a torre que pode ser constru�da neste bloco
    private Color startColor; // Armazena a cor original do bloco para restaurar quando o mouse sai

    private void Start()
    {
        // Salva a cor original do bloco ao iniciar o jogo
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        // Muda a cor do bloco para 'hoverColor' quando o mouse entra na �rea do bloco
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        // Restaura a cor original do bloco quando o mouse sai da �rea do bloco
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // Verifica se j� existe uma torre neste bloco
        if (tower != null) return;

        // Obt�m a torre selecionada atrav�s do BuildManager
        Tower towerToBuild = BuildManager.Instance.GetSelectedTower();

        // Verifica se h� moeda suficiente para construir a torre
        if (towerToBuild.coast > LevelManager.main.currency)
        {
            return; // N�o constr�i a torre se n�o houver moedas suficientes
        }

        // Deduz o custo da constru��o da torre da moeda dispon�vel
        LevelManager.main.SpendCurency(towerToBuild.coast);

        // Instancia a torre na posi��o do bloco, atribuindo-a ao campo 'tower'
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
