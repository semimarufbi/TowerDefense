using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    [Header("Attributes")]

    private int selectedTower = 0;



    private void Awake()
    {
        Instance = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}

