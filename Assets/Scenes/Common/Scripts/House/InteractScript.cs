using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class InteractScript : MonoBehaviour
{
    public Interactables[] interactables;
    int currentInteractable = 0;
    public bool interacted;
    public bool finishedAnimation;
    public bool allTasksCompleted;
    public bool resetPosition;
    public float timeSinceInteracted = 0;
    public int minusOne;
    public GameObject watchNotifications;
    public GameObject LoadingScreen;
    public string nextSceneName;

    // Update is called once per frame
    void Update()
    {
        if (interactables[currentInteractable].trigger.GetComponent<Trigger>().entered == true && Input.GetButtonDown("Interact"))
        {
            interacted = true;
        }
        if (interacted == true)
        {
            timeSinceInteracted += Time.fixedDeltaTime;

            if (interactables[currentInteractable].animator != null)
            {
                interactables[currentInteractable].animator.SetBool("Activate", true);
                if (interactables[currentInteractable].animator.GetCurrentAnimatorStateInfo(0).IsName("Finished") && finishedAnimation == false)
                {
                    finishedAnimation = true;
                    interactables[currentInteractable].interactable.SetActive(false);
                    TaskCompleted();
                }
            }
            if (interactables[currentInteractable].portable == true)
            {
                resetPosition = false;
                interactables[currentInteractable].interactable.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                interactables[currentInteractable].interactable.transform.localPosition = Vector3.Slerp(interactables[currentInteractable].interactable.transform.localPosition, interactables[currentInteractable].offset, interactables[currentInteractable].pickupTime);

                if (interactables[currentInteractable].bringToTrigger)
                {
                    if (interactables[currentInteractable].bringTrigger.GetComponent<Trigger>().entered == true)
                    {
                        TaskCompleted();
                    }
                }
            }
            if (interactables[currentInteractable].freezeState == true)
            {

                if (interactables[currentInteractable].freezeDuration < timeSinceInteracted)
                {
                    TaskCompleted();
                } else
                {
                    GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.Slerp(GameObject.FindGameObjectWithTag("Player").transform.position, interactables[currentInteractable].freezePosition, interactables[currentInteractable].freezeTransition);
                }
            }
            if (interactables[currentInteractable].changeSprite == true)
            {
                interactables[currentInteractable].spriteToChange.GetComponent<SpriteRenderer>().sprite = interactables[currentInteractable].sprite;
                interactables[currentInteractable].spriteToChange.SetActive(true);
                if (interactables[currentInteractable].freezeState == false)
                {
                    TaskCompleted();
                }
            }
            if (interactables[currentInteractable].animator == null && interactables[currentInteractable].portable == false && interactables[currentInteractable].freezeState == false && interactables[currentInteractable].changeSprite == false)
            {
                TaskCompleted();
            }
        }
        if (resetPosition == true)
        {
            interactables[currentInteractable +minusOne].interactable.transform.parent = null;
            interactables[currentInteractable +minusOne].interactable.transform.position = Vector3.Slerp(interactables[currentInteractable +minusOne].interactable.transform.position, interactables[currentInteractable +minusOne].endingLocation, interactables[currentInteractable +minusOne].pickupTime);
        }
    }

    void TaskCompleted()
    {
        ResetVariables();

        if (currentInteractable + 1 < interactables.Length && currentInteractable + 1 != interactables.Length)
        {
            currentInteractable += 1;
            watchNotifications.GetComponent<Notifications>().notiNum += 1;
        }
        else
        {
            if (allTasksCompleted == false)
            {
                watchNotifications.GetComponent<Notifications>().notiNum += 1;
                allTasksCompleted = true;
            } else
            {
                LoadingScreen.GetComponent<LoadingScreenManager>().LoadLevel(nextSceneName);
            }
        }
    }

    void ResetVariables()
    {
        if (interactables[currentInteractable].portable)
        {
            resetPosition = true;
        }
        finishedAnimation = false;
        interacted = false;
        timeSinceInteracted = 0;
    }
}