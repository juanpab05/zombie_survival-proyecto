
using UnityEngine;
using TMPro;

interface IInteractable
{
    public void Interact(string tag);
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 0.5f;
    public TMP_Text InteractionPrompt;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        SetPromptVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange) &&
            hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            string hitTag = hitInfo.collider.gameObject.tag;
            SetPromptVisible(true);
            if (InteractionPrompt != null)
            {
                InteractionPrompt.text = $"Press E to pickup {hitTag}";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactObj.Interact(hitTag);
                anim.SetInteger("Melee_w", 1);
            }
        }
        else
        {
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
}
