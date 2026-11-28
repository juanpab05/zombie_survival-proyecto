using UnityEngine;
using TMPro;
using System.Collections;

public class OpenDoor : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;
    public string interactMessage = "Press E to interact with door";
    public string animInteractTrigger = "Attack";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInHitbox)
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(ToggleDoor());
        }
    }*/

    public void Interact(string tag)
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(ToggleDoor());
    }

    public string getInteractText()
    {
        return interactMessage;
    }

    public string getInteractAnimationTrigger()
    {
        return animInteractTrigger;
    }


    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
