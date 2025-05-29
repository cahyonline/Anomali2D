using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemyWall : MonoBehaviour
{
    // Start is called before the first frame update
    private bool startBabel = false;
    public EnemyAIRanged enemyAIRanged;
    void Start()
    {
        startBabel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAIRanged.healthAmount <= 0f)
        {
            Debug.Log("Call Luthvy for this");
        }
    }
}
