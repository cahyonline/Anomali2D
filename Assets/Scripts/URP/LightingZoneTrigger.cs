using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingZoneTrigger : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerLight;

    public float globalLightIntensityInZone = 0f;
    public float globalLightIntensityOutZone = 34f;

    public float defaultIntensity = 0.37f;
    public float defaultInnerRadius = 1.1f;
    public float defaultOuterRadius = 12.03f;
    public float defaultFalloff = 1f;

    public float itemIntensity = 1f;
    public float itemInnerRadius = 3f;
    public float itemOuterRadius = 18f;
    public float itemFalloff = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            globalLight.intensity = globalLightIntensityInZone;

            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.HasItem(ItemType.LightSource))
            {
                playerLight.intensity = itemIntensity;
                playerLight.pointLightInnerRadius = itemInnerRadius;
                playerLight.pointLightOuterRadius = itemOuterRadius;
                playerLight.falloffIntensity = itemFalloff;
            }
            else
            {
                playerLight.intensity = defaultIntensity;
                playerLight.pointLightInnerRadius = defaultInnerRadius;
                playerLight.pointLightOuterRadius = defaultOuterRadius;
                playerLight.falloffIntensity = defaultFalloff;
            }

            playerLight.enabled = true;
        }
    }
}
