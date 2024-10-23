using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigos : MonoBehaviour
{
    [Header("atributos")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb;

    private Transform alvo;

    private int pathIndex = 0;


    private void Start()
    {
        alvo = LevelManager.main.path[0];
    }
    private void Update()
    {
        if (Vector2.Distance(alvo.position,transform.position) <= 0.1f)
        {
            pathIndex++;
            if(pathIndex == LevelManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }
        }
    }


}
