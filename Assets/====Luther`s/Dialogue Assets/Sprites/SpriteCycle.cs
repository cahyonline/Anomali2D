using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCycle : MonoBehaviour
{
    public Image portraitImage;               // UI Image to display the sprite
    public Sprite[] characterSprites;         // Assign your sprites in Inspector

    private int currentIndex = 0;

    void Start()
    {
        if (characterSprites.Length > 0 && portraitImage != null)
        {
            portraitImage.sprite = characterSprites[currentIndex];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CyclePortrait();
        }
    }

    void CyclePortrait()
    {
        if (characterSprites.Length == 0 || portraitImage == null)
            return;

        currentIndex++;
        if (currentIndex >= characterSprites.Length)
        {
            currentIndex = 0;
            Debug.LogWarning("CYCLED BACK");
        }
            


        portraitImage.sprite = characterSprites[currentIndex];
        Debug.Log("Switched to portrait index: " + currentIndex);
    }
}