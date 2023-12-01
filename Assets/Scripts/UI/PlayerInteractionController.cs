using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    public float interactionRange = 2f;

    private List<InteractableTarget> targets;
    private Dictionary<KeyCode, UnityEvent> availableActions;
    
    [Header("HUD Objects")]
    [SerializeField]
    private Transform promptPrefab;
    [SerializeField]
    private Transform promptParent;

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
        if(Time.timeScale <= 0) return;
        
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
                optText.text = $"{option.interactionName} [{option.input.ToString()}]";
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
    }
}
