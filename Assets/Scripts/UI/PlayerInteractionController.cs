using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Player;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement player;

    public float interactionRange = 2f;

    private List<InteractableTarget> targets;
    private Dictionary<KeyCode, UnityEvent> availableActions;
    
    [Header("HUD Objects")]
    [SerializeField]
    private Transform promptPrefab;
    [SerializeField]
    private Transform promptParent;
    
    [Header("Player-related references")]
    [SerializeField] private CharacterMovement charMov;
    [SerializeField] private Transform inventoryScreen;

    private void Awake()
    {
        targets = new List<InteractableTarget>();
        
        foreach (InteractableTarget targ in FindObjectsOfType<InteractableTarget>())
        {
            targets.Add(targ);
        }

        promptParent.gameObject.SetActive(false);
        availableActions = new Dictionary<KeyCode, UnityEvent>();
    }

    private void Update()
    {
        if(!player.canMove) return;
        
        //Find nearest interactable target
        InteractableTarget nearest = targets.OrderBy(t => Vector2.Distance(player.transform.position, t.transform.position))
            .FirstOrDefault();

        if (nearest == null || Vector2.Distance(player.transform.position, nearest.transform.position) > interactionRange)
        {
            if (promptParent.gameObject.activeSelf)
            {
                promptParent.gameObject.SetActive(false);
                availableActions = new Dictionary<KeyCode, UnityEvent>();
            }
            return;
        }
        
        //Draw prompts
        if (!promptParent.gameObject.activeSelf)
        {
            if (promptParent.childCount > 0)
            {
                foreach (Transform t in promptParent.transform.GetComponentsInChildren<Transform>())
                {
                    if (t == promptParent) continue;
                    Destroy(t.gameObject);
                }    
            }
        
            foreach (InteractionOptions option in nearest.options)
            {
                Transform opt = Instantiate(promptPrefab, promptParent);
                TMP_Text optText = opt.GetComponentInChildren<TMP_Text>();
                optText.text = option.input != KeyCode.None ? $"{option.interactionName} [{option.input.ToString()}]" : $"{option.interactionName}" ;
                availableActions.Add(option.input, option.OnActionSelected);
            }
            
            promptParent.gameObject.SetActive(true);
        }

        foreach (KeyValuePair<KeyCode, UnityEvent> action in availableActions)
        {
            if (Input.GetKeyDown(action.Key))
            {
                action.Value?.Invoke();
                promptParent.gameObject.SetActive(false);
                availableActions = new Dictionary<KeyCode, UnityEvent>();
            }
        }
        
        if(charMov == null || !charMov.isPlayer) return;
        if(inventoryScreen == null) return;
        
        if (Input.GetKeyDown(KeyCode.I) && charMov.canMove)
        {
            inventoryScreen.gameObject.SetActive(true);
            if (promptParent.gameObject.activeSelf)
            {
                promptParent.gameObject.SetActive(false);
                availableActions = new Dictionary<KeyCode, UnityEvent>();
            }
        }
    }
}
