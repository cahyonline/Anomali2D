using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject Player; // GameObject player
    [SerializeField] private List<Transform> SpawnPoints; // List posisi spawn untuk setiap area

    private int currentAreaIndex = 0; // Indeks area saat ini

    private IEnumerator GetSpawn(int areaIndex)
    {
        yield return new WaitForSeconds(0.2f); 

        if (Player != null && SpawnPoints != null && areaIndex >= 0 && areaIndex < SpawnPoints.Count)
        {
            Player.transform.position = SpawnPoints[areaIndex].position;

            if (areaIndex > currentAreaIndex)
            {
                //Debug.Log("Player berpindah ke NEXT SPAWN: " + areaIndex);
            }
            else if (areaIndex < currentAreaIndex)
            {
                Debug.Log("Player berpindah ke PREV SPAWN: " + areaIndex);
            }
            else
            {
                Debug.Log("Player tetap di area yang sama: " + areaIndex);
            }

            currentAreaIndex = areaIndex; // Update indeks area saat ini
        }
        else
        {
            Debug.LogWarning("Player, SpawnPoints, atau areaIndex tidak valid!");
        }
    }

    private void Spawner(int areaIndex)
    {
        StartCoroutine(GetSpawn(areaIndex)); // Mulai coroutine untuk spawn
    }

    private void OnEnable()
    {
        EventCallBack.ChangeAreaSpawn += Spawner; // Daftarkan event ChangeAreaSpawn
    }

    private void OnDisable()
    {
        EventCallBack.ChangeAreaSpawn -= Spawner; // Hapus event ChangeAreaSpawn
    }
}