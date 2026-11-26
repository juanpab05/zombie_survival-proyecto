using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryHotbar : MonoBehaviour
{
    [Header("Slots (5 espacios estilo Minecraft)")]
    public ItemData[] slots = new ItemData[5];
    public int selectedIndex = 0;

    [Header("Referencias UI del Canvas")]
    public Image[] slotFrames;
    public Image[] slotIcons;
    public Color unselectedColor = new Color(0.15f, 0.15f, 0.15f, 0.75f);
    public Color selectedColor = new Color(1f, 0.85f, 0.2f, 1f);

    [Header("Referencias del Jugador y Armas")]
    public HealthSystem healthSystem;
    public GameObject[] weaponModels;

    private void Start()
    {
        if (healthSystem == null)
            healthSystem = GetComponent<HealthSystem>();

        UpdateHotbarUI();
        UpdateHandVisuals();
    }

    private void Update()
    {
        HandleSelectionInput();
        HandleUseItemInput();
    }

    private void HandleSelectionInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectSlot(4);

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0f)
                SelectSlot((selectedIndex - 1 + slots.Length) % slots.Length);
            else if (scroll < 0f)
                SelectSlot((selectedIndex + 1) % slots.Length);
        }
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        selectedIndex = index;
        UpdateHotbarUI();
        UpdateHandVisuals();
    }

    private void HandleUseItemInput()
    {
        bool usePressed = (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
                       || (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame);

        if (usePressed)
        {
            ItemData current = slots[selectedIndex];
            if (current != null && current.itemType == ItemType.Consumable)
            {
                HealPlayer(current);
            }
        }
    }

    private void HealPlayer(ItemData item)
    {
        if (healthSystem == null || healthSystem.IsDead) return;

        healthSystem.Heal(item.healAmount);

        slots[selectedIndex] = null;
        UpdateHotbarUI();
        UpdateHandVisuals();
    }

    public bool AddItem(ItemData newItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = newItem;
                UpdateHotbarUI();
                UpdateHandVisuals();
                return true;
            }
        }
        return false;
    }

    private void UpdateHandVisuals()
    {
        ItemData current = slots[selectedIndex];

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
                weaponModels[i].SetActive(false);
        }

        if (current != null && current.itemType == ItemType.Weapon)
        {
            if (current.weaponIndex >= 0 && current.weaponIndex < weaponModels.Length)
            {
                if (weaponModels[current.weaponIndex] != null)
                    weaponModels[current.weaponIndex].SetActive(true);
            }
        }
    }

    public void UpdateHotbarUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < slotFrames.Length && slotFrames[i] != null)
                slotFrames[i].color = (i == selectedIndex) ? selectedColor : unselectedColor;

            if (i < slotIcons.Length && slotIcons[i] != null)
            {
                if (slots[i] != null && slots[i].icon != null)
                {
                    slotIcons[i].sprite = slots[i].icon;
                    slotIcons[i].enabled = true;
                }
                else
                {
                    slotIcons[i].sprite = null;
                    slotIcons[i].enabled = false;
                }
            }
        }
    }
}