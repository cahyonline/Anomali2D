using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterInteract : MonoBehaviour
{
    [SerializeField] private PlayerAnimator animHandler;
    private void Update()
    {
        if (Input.anyKeyDown && GamesState.InInteract)
        {
            animHandler.AfterInteract = true;
            Debug.Log("Ada tombol yang ditekan!");
        }

    }
}
