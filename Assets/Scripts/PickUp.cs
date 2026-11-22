using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    public void Interact(string tag)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Cannot pick up weapon: no GameObject with the Player tag was found.");
            return;
        }

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

        Debug.LogWarning($"Cannot pick up weapon: no inactive object with tag '{tag}' was found under the Player.");
    }
}
