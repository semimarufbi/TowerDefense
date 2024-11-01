using System;
using UnityEngine;

[Serializable]
public class Tower 
{
    public string name;
    public int coast;
    public GameObject prefab;

    public Tower (string _name, int _coast, GameObject _prefab)
    {
        this.name = _name;
        this.coast = _coast;
        this.prefab = _prefab;
    }
}
