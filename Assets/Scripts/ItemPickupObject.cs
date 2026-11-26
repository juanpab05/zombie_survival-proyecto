using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickupObject : MonoBehaviour
{
    public ItemData itemData;
    private bool playerInRange = false;
    private InventoryHotbar playerInventory;

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (playerInventory != null && playerInventory.AddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<InventoryHotbar>();
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}