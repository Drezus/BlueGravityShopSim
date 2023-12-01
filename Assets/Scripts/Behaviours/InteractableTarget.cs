using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct InteractionOptions
{
    public string interactionName;
    public KeyCode input;
    public UnityEvent OnActionSelected;
}

public class InteractableTarget : MonoBehaviour
{
    public List<InteractionOptions> options;
}
