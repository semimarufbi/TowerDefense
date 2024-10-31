using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;

    public int currency;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        currency = 100;
    }

    public void IncreaseCurerency(int amanunt)
    {
        currency += amanunt;
    }

    public bool SpendCurency(int amanunt) 
    {
        if (amanunt <= currency)
        {
            currency -= amanunt;
            return true;
        }
        else
        {
            return false;
        }
    }
}
