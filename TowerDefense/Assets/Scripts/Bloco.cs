using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloco : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    [Header("Attributes")]
    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;

        Tower towerToBuild = BuildManager.Instance.GetSelectedTower();

        // Instanciando a torre corretamente
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity); // Atribuindo a torre ao campo 'tower'
    }
}
