using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    public string interactText = "E to Use";

    public UnityEvent OnUse;

    public void Use()
    {
        OnUse.Invoke();
    }
}
