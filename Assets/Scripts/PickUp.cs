using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    [Header("Ítem para el inventario (Hotbar)")]
    public ItemData itemData;
    public string interactMessage;
    public string itemTag;

    public string animTrigger = "Pickup";

    public string getInteractText()
    {  
        itemTag = gameObject.tag;
        interactMessage = $"Press E to pickup {itemTag}";
        return interactMessage;
    }

    public string getAnimationTrigger()
    {
        return animTrigger;
    }

    public void Interact(string tag)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag Player.");
            return;
        }

        InventoryHotbar inventory = player.GetComponent<InventoryHotbar>();

        // 1. Si el objeto tiene un ItemData asignado, intenta meterlo en la barra
        if (inventory != null && itemData != null)
        {
            if (!inventory.AddItem(itemData))
            {
                Debug.Log("El inventario está lleno.");
                return; // Si la barra está llena, no destruye el objeto del suelo
            }

            Destroy(gameObject);
            return;
        }

        // 2. Respaldo original (por si algún objeto en la escena no tiene ItemData asignado)
        Transform[] children = player.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.CompareTag(tag))
            {
                child.gameObject.SetActive(true);
                Destroy(gameObject);
                return;
            }
        }

        Debug.LogWarning($"No se encontró objeto inactivo con tag '{tag}' bajo el Player.");
    }
}
