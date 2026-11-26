
using UnityEngine;
using TMPro;
using System.Collections;

interface IInteractable
{
    public void Interact(string tag);
}

public class Interactor : MonoBehaviour
{
    public TMP_Text InteractionPrompt;
    private Animator anim;
    private bool isPickingUp;
    private IInteractable currentInteractable;
    private string currentInteractableTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        SetPromptVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null && !isPickingUp)
        {
            anim.SetTrigger("Pickup");
            StartCoroutine(CompletePickup(currentInteractable, currentInteractableTag));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            currentInteractableTag = other.tag;

            SetPromptVisible(true);
            if (InteractionPrompt != null)
            {
                InteractionPrompt.text = $"Press E to pickup {currentInteractableTag}";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable) &&
            interactable.Equals(currentInteractable))
        {
            currentInteractable = null;
            currentInteractableTag = null;
            SetPromptVisible(false);
        }
    }

    private void SetPromptVisible(bool visible)
    {
        if (InteractionPrompt != null)
        {
            InteractionPrompt.gameObject.SetActive(visible);
        }
    }

    private IEnumerator CompletePickup(IInteractable interactObj, string hitTag)
    {
        isPickingUp = true;
        SetPromptVisible(false);

        yield return new WaitForSeconds(0.5f);

        interactObj.Interact(hitTag);
        anim.SetInteger("Melee_w", 1);
        isPickingUp = false;
    }
}
