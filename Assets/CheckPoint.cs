using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    public Transform respawnPoint;

    private void Awake()
    {
        //gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!gameController)
            Debug.LogError("Checkpoint");
            //gameController.UpdateCheckpoint(respawnPoint.position);
        }
    }
}
