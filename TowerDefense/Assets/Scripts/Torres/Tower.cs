using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name; // Nome da torre
    public int coast; // Custo da torre
    public GameObject prefab; // Prefab associado à torre

    // Construtor para inicializar a torre com nome, custo e prefab
    public Tower(string _name, int _coast, GameObject _prefab)
    {
        this.name = _name;
        this.coast = _coast;
        this.prefab = _prefab;
    }
}
